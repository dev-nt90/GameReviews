using FluentValidation;
using GameRatings.Contracts.Responses;


namespace GameRatings.Api.Middleware
{
	public class ValidationMappingMiddleware
	{
		private readonly RequestDelegate next;

		public ValidationMappingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch(ValidationException ve)
			{
				context.Response.StatusCode = 400;
				var validationFailureResponse = new ValidationFailureResponse
				{
					Errors = ve.Errors.Select(e => new ValidationResponse
					{
						PropertyName = e.PropertyName,
						Message = e.ErrorMessage
					})
				};

				await context.Response.WriteAsJsonAsync(validationFailureResponse);
			}
		}
	}
}
