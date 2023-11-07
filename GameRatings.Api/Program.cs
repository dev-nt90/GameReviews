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

using GameRatings.Api.Middleware;
using GameRatings.Application;
using GameRatings.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseMiddleware<RouteDiagnosticsMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInit = app.Services.GetRequiredService<DbInitializer>();
await dbInit.InitializeAsync();

app.Run();
