using System.Security.Claims;
using System.Text.Json;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;

namespace HeThongHoTroVaQuanLyPhongKham.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || 
                context.Request.Path.StartsWithSegments("/api/auth/register"))
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity.IsAuthenticated)
                throw new UnauthorizedException("Bạn không có quyền truy cập vì chưa đăng nhập.");

            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
                throw new ForbiddenException("Bạn không có quyền truy cập trang này vì không có quyền hợp lệ.");                

            await _next(context);
        }
    }
}
