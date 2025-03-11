using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/symptoms")]
    [ApiController]
    public class TrieuChungController : ControllerBase
    {
        private readonly IService<TrieuChungDTO> _trieuChungService;

        public TrieuChungController(IService<TrieuChungDTO> trieuChungService)
        {
            _trieuChungService = trieuChungService;
        }


        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,KyThuatVienXetNghiem")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<TrieuChungDTO>>.Success(
                    await _trieuChungService.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,KyThuatVienXetNghiem")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<TrieuChungDTO>.Success(
                    await _trieuChungService.GetByIdAsync(id), $"Tìm thấy triệu chứng với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Create([FromBody] TrieuChungDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaTrieuChung },
                    ApiResponse<TrieuChungDTO>.Success(
                        await _trieuChungService.AddAsync(dto), "Thêm triệu chứng thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TrieuChungDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaTrieuChung)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<TrieuChungDTO>.Success(
                    await _trieuChungService.UpdateAsync(dto), $"Cập nhật triệu chứng với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _trieuChungService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TrieuChungDTO>.Fail(ex.Message));
            }
        }
    }
}
