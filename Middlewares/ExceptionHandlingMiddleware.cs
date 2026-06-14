using FinTrack.Common;
using FinTrack.Common.Exceptions;

namespace FinTrack.Middlewares
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhabdled Exception occurred");

                var (statusCode, message) = ex switch
                {
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
                    ArgumentException => (StatusCodes.Status400BadRequest, ex.Message),
                    ExternalServiceException => (StatusCodes.Status503ServiceUnavailable, "External service is currently unavailable. Please try again later."),
                    _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred.")
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(
                    new ErrorResponse(statusCode,message)
                );
            }
        }
    }
}