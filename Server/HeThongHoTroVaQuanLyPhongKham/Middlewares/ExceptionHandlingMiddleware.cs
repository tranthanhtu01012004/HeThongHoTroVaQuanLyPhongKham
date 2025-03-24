using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using System.Text.Json;

namespace HeThongHoTroVaQuanLyPhongKham.Middlewares
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
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning("Unauthorized: {Message}", ex.Message);
                await HandleUnauthorizedAsync(context, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                _logger.LogWarning("Forbidden: {Message}", ex.Message);
                await HandleForbiddenAsync(context, ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Not Found: {Message}", ex.Message);
                await HandleNotFoundAsync(context, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                await HandleGeneralExceptionAsync(context, "An unexpected error occurred.");
            }
        }

        private async Task HandleUnauthorizedAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var errorResponse = new { status = 401, error = "Unauthorized", message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleNotFoundAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            var errorResponse = new { status = 404, error = "Not Found", message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleForbiddenAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            var errorResponse = new { status = 403, error = "Forbidden", message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleGeneralExceptionAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = new { status = 500, error = "Internal Server Error", message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
