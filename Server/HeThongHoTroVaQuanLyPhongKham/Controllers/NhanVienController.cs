using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/employees")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;

        public NhanVienController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _nhanVienService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<NhanVienDTO>>.Success(items, page, pageSize, totalPages, totalItems, $"Đã lấy danh sách nhân viên - trang {page} với {pageSize} bản ghi."));
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

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotPaginator()
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<NhanVienDTO>>.Success(
                    await _nhanVienService.GetAllAsync(), 
                    $"Đã lấy danh sách nhân viên"));
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

        [HttpGet("{id:int}")]
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
        public async Task<IActionResult> Create([FromBody] NhanVienDTO nhanVienDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = nhanVienDTO.MaNhanVien }, 
                    ApiResponse<NhanVienDTO>.Success(
                        await _nhanVienService.AddAsync(nhanVienDTO), "Thêm nhân viên thành công."));
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NhanVienDTO NhanVienDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != NhanVienDTO.MaNhanVien)
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

        [HttpDelete("{id:int}")]
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
