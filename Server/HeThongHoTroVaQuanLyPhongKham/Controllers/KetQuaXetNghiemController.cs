using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/test-results")]
    [ApiController]
    public class KetQuaXetNghiemController : ControllerBase
    {
        private readonly IService<KetQuaXetNghiemDTO> _ketQuaXetNghiemService;

        public KetQuaXetNghiemController(IService<KetQuaXetNghiemDTO> ketQuaXetNghiemService)
        {
            _ketQuaXetNghiemService = ketQuaXetNghiemService;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,KyThuatVienXetNghiem")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<KetQuaXetNghiemDTO>>.Success(
                    await _ketQuaXetNghiemService.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,KyThuatVienXetNghiem")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<KetQuaXetNghiemDTO>.Success(
                    await _ketQuaXetNghiemService.GetByIdAsync(id), $"Tìm thấy kết quả xét nghiệm với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,KyThuatVienXetNghiem")]
        public async Task<IActionResult> Create([FromBody] KetQuaXetNghiemDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaKetQua },
                    ApiResponse<KetQuaXetNghiemDTO>.Success(
                        await _ketQuaXetNghiemService.AddAsync(dto), "Thêm kết quả xét nghiệm thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,KyThuatVienXetNghiem")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] KetQuaXetNghiemDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaKetQua)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<KetQuaXetNghiemDTO>.Success(
                    await _ketQuaXetNghiemService.UpdateAsync(dto), $"Cập nhật kết quả xét nghiệm với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,KyThuatVienXetNghiem")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _ketQuaXetNghiemService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaXetNghiemDTO>.Fail(ex.Message));
            }
        }
    }
}
