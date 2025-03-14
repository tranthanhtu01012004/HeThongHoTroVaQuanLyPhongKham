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

            // Lấy tất cả vai trò của người dùng
            var userRoles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (userRoles == null || !userRoles.Any())
                throw new ForbiddenException("Bạn không có quyền truy cập trang này vì tài khoản chưa có vai trò.");

            // Lấy danh sách vai trò yêu cầu từ endpoint
            var requiredRolesAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();
            if (requiredRolesAttribute?.Roles != null)
            {
                var requiredRoles = requiredRolesAttribute.Roles.Split(',').Select(r => r.Trim()).ToList();

                bool hasRequiredRole = userRoles.Any(userRole => requiredRoles.Contains(userRole));
                if (!hasRequiredRole)
                    throw new ForbiddenException("Bạn không có quyền truy cập trang này vì vai trò không phù hợp với yêu cầu.");
            }

            await _next(context);
        }
    }
}
