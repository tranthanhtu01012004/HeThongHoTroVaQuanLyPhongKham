using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IService<TaiKhoanDTO> _taiKhoanService;

        public AuthController(IAuthService authService, IService<TaiKhoanDTO> taiKhoanService)
        {
            _authService = authService;
            _taiKhoanService = taiKhoanService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TaiKhoanDTO taiKhoanDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loginResponse = await _authService.Login(taiKhoanDTO);
                return Ok(ApiResponse<LoginResponse>.Success(loginResponse, "Đăng nhập thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong quá trình đăng nhập." });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] TaiKhoanDTO taiKhoanDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(ApiResponse<TaiKhoanDTO>.Success(
                    await _taiKhoanService.AddAsync(taiKhoanDTO), "Đăng ký tài khoản thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            } catch(DuplicateEntityException ex)
            {
                return Conflict(ApiResponse<TaiKhoanDTO>.Fail(ex.Message)); // 409: Tai nguyen dang yeu cau da ton tai, gay xung dot voi trang thai hien tai cua he thong.
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }
    }
}
