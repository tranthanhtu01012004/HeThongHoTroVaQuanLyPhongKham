using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly IService<NhanVienDTO> _nhanVienService;

        public NhanVienController(IService<NhanVienDTO> nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<NhanVienDTO>>.Success(
                    await _nhanVienService.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<NhanVienDTO>.Success(
                    await _nhanVienService.GetByIdAsync(id), $"Tìm thấy nhân viên với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienDTO NhanVienDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var taiKhoan = await _nhanVienService.AddAsync(NhanVienDTO);

                return Ok(ApiResponse<NhanVienDTO>.Success(taiKhoan, "Đăng ký nhân viên thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NhanVienDTO NhanVienDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != NhanVienDTO.MaTaiKhoan)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<NhanVienDTO>.Success(
                    await _nhanVienService.UpdateAsync(NhanVienDTO), $"Cập nhật nhân viên với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _nhanVienService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }
    }
}
