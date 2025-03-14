using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/medicines")]
    [ApiController]
    public class ThuocController : ControllerBase
    {
        private readonly IService<ThuocDTO> _thuocService;

        public ThuocController(IService<ThuocDTO> thuocService)
        {
            _thuocService = thuocService;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _thuocService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<ThuocDTO>>.Success(items, page, pageSize, totalPages, totalItems, $"Đã lấy danh sách thuốc - trang {page} với {pageSize} bản ghi."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<ThuocDTO>.Success(
                    await _thuocService.GetByIdAsync(id), $"Tìm thấy thuốc với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,DuocSi")]
        public async Task<IActionResult> Create([FromBody] ThuocDTO thuocDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = thuocDto.MaThuoc }, 
                    ApiResponse<ThuocDTO>.Success(
                        await _thuocService.AddAsync(thuocDto), "Thêm thuốc mới thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,DuocSi")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ThuocDTO ThuocDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != ThuocDTO.MaThuoc)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<ThuocDTO>.Success(
                    await _thuocService.UpdateAsync(ThuocDTO), $"Cập nhật dịch vụ y tế với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,DuocSi")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _thuocService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ThuocDTO>.Fail(ex.Message));
            }
        }
    }
}
