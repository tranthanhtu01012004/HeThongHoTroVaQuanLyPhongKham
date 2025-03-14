using System.Security.Claims;
using System.Text.Json;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using Microsoft.AspNetCore.Authorization;

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
            // Nếu API có [AllowAnonymous], bỏ qua kiểm tra xác thực
            var endpoint = context.GetEndpoint();
            var hasAllowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;
            if (hasAllowAnonymous)
            {
                await _next(context);
                return;
            }

            // Nếu không có token => trả về 401
            if (!context.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedException("Bạn không có quyền truy cập vì chưa đăng nhập.");
            }

            // Kiểm tra role (nếu API yêu cầu)
            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                throw new ForbiddenException("Bạn không có quyền truy cập trang này vì tài khoản chưa có vai trò.");
            }

            await _next(context);
        }
    }
}
