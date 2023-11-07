/*
 * The idea: download steam review data, load it up into a database, formulate an API and presentation layer, and 
 * present some analytics about the reviews. Make it available by title, genre, year of release, or any combination thereof.
 * 
 * example request for store data: 
 * https://store.steampowered.com/appreviews/939100?
 * cursor=*&day_range=30&
 * start_date=-1&
 * end_date=-1&
 * date_range_type=all&
 * filter=summary&
 * language=english&
 * l=english&
 * review_type=all&
 * purchase_type=all&
 * playtime_filter_min=0&
 * playtime_filter_max=0&
 * filter_offtopic_activity=1&
 * summary_num_positive_reviews=319&
 * summary_num_reviews=468&
 * json=1
 * 
 * from https://www.reddit.com/r/Steam/comments/f79dd2/steam_database_with_users_games_reviews_where_can/
 */

using GameRatings.Api;
using GameRatings.Api.Middleware;
using GameRatings.Application;
using GameRatings.Application.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => 
{
	x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
	{
		// TODO: do not store secrets in configuration
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
		ValidIssuer = config["Jwt:Issuer"],
		ValidAudience = config["Jwt:Audience"],
		ValidateIssuer = true,
		ValidateAudience = true
	};
});

builder.Services.AddAuthorization(x =>
{
	x.AddPolicy(AuthConstants.AdminUserPolicyName, p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

	x.AddPolicy(AuthConstants.TrustedMemberPolicyName,
		p => p.RequireAssertion(c =>
		c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
		c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" })));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]); // TODO: store secrets more goodly

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseMiddleware<RouteDiagnosticsMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

// HACK: fake database until datagen is ready
var dbInit = app.Services.GetRequiredService<DbInitializer>();
await dbInit.InitializeAsync();

app.Run();
