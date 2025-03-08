using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        /// <summary>
        /// Đăng nhập hệ thống và trả về token JWT cùng vai trò.
        /// </summary>
        /// <param name="taiKhoanDTO">Thông tin đăng nhập (tên đăng nhập và mật khẩu).</param>
        /// <returns>Token và danh sách vai trò nếu đăng nhập thành công.</returns>
        /// <response code="200">Đăng nhập thành công, trả về token và vai trò.</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="401">Tên đăng nhập hoặc mật khẩu không đúng.</response>
        /// <response code="404">Không tìm thấy tài khoản.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] TaiKhoanDTO taiKhoanDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var loginResponse = await _authService.Login(taiKhoanDTO);
                return Ok(loginResponse);
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
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong quá trình đăng nhập.", Details = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] TaiKhoanDTO taiKhoanDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var taiKhoan = await _taiKhoanService.AddAsync(taiKhoanDTO);

                return Ok(ApiResponse<TaiKhoanDTO>.Success(taiKhoan, "Đăng ký tài khoản thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }
    }
}
