namespace GameRatings.Api.Middleware
{
	public class RouteDiagnosticsMiddleware
	{
		private readonly RequestDelegate _next;

		public RouteDiagnosticsMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var routeData = context.GetRouteData();
			if(routeData != null)
			{
				LogRouteData(routeData);
			}
			await _next(context);
		}

		private void LogRouteData(RouteData routeData) 
		{
			Console.WriteLine($"Route data: {routeData}");
		}
	}
}
