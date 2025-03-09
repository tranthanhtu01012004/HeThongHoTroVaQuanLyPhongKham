using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using System.Text.Json;

namespace HeThongHoTroVaQuanLyPhongKham.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedException ex)
            {
                await HandleUnauthorizedAsync(context, ex.Message);
            } 
            catch (ForbiddenException ex)
            {
                await HandleForbiddenAsync(context, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundAsync(context, ex.Message);
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
    }
}
