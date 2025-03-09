using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/prescriptions")]
    [ApiController]
    public class DonThuocController : ControllerBase
    {
        private readonly IDonThuocService _donThuocService;

        public DonThuocController(IDonThuocService donThuocService)
        {
            _donThuocService = donThuocService;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi,YTa")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<DonThuocDTO>>.Success(
                    await _donThuocService.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi,YTa")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<DonThuocDTO>.Success(
                    await _donThuocService.GetByIdAsync(id), $"Tìm thấy đơn thuốc với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Create([FromBody] DonThuocDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaDonThuoc },
                    ApiResponse<DonThuocDTO>.Success(
                        await _donThuocService.AddAsync(dto), "Thêm đơn thuốc thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DonThuocDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaDonThuoc)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<DonThuocDTO>.Success(
                    await _donThuocService.UpdateAsync(dto), $"Cập nhật đơn thuốc với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _donThuocService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocDTO>.Fail(ex.Message));
            }
        }
    }
}
