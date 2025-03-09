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
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || 
                context.Request.Path.StartsWithSegments("/api/auth/register"))
            {
                await _next(context);
                return;
            }

            // Kiểm tra xác thực và phân quyền
            if (context.Request.Path.StartsWithSegments("/api/admin"))
            {
                if (!context.User.Identity.IsAuthenticated)
                    throw new UnauthorizedException("Bạn không có quyền truy cập vì chưa đăng nhập.");

                var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(role))
                    throw new ForbiddenException("Bạn không có quyền truy cập trang này vì tài khoản chưa có vai trò, vui lòng liên hệ admin để gán vai trò.");

                // Lấy metadata của endpoint
                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    // Lấy thông tin phân quyền từ metadata
                    var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();
                    if (authorizeData.Any())
                    {
                        // Nếu endpoint yêu cầu phân quyền, kiểm tra vai trò
                        var allowedRoles = authorizeData
                            .Where(a => !string.IsNullOrEmpty(a.Roles))
                            .SelectMany(a => a.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries))
                            .Select(r => r.Trim())
                            .ToList();

                        // Nếu có danh sách vai trò được phép nhưng vai trò của người dùng không nằm trong đó
                        if (allowedRoles.Any() && !allowedRoles.Contains(role))
                            throw new ForbiddenException("Bạn không có quyền thực hiện hành động này.");
                    }
                }
            }

            await _next(context);
        }
    }
}
