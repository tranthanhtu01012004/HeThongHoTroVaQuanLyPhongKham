using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/healthcare-services")]
    [ApiController]
    public class DichVuYTeController : ControllerBase
    {
        private readonly IService<DichVuYTeDTO> _dichVuYTeService;

        public DichVuYTeController(IService<DichVuYTeDTO> dichVuYTeService)
        {
            _dichVuYTeService = dichVuYTeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _dichVuYTeService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<DichVuYTeDTO>>.Success(items, page, pageSize, totalPages, totalItems));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,KyThuatVienXetNghiem")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<DichVuYTeDTO>.Success(
                    await _dichVuYTeService.GetByIdAsync(id), $"Tìm thấy dịch vụ y tế với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Create([FromBody] DichVuYTeDTO dichVuYTeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = dichVuYTeDTO.MaDichVuYTe }, 
                    ApiResponse<DichVuYTeDTO>.Success(
                        await _dichVuYTeService.AddAsync(dichVuYTeDTO), "Thêm dịch vụ y tế thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DichVuYTeDTO DichVuYTeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != DichVuYTeDTO.MaDichVuYTe)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<DichVuYTeDTO>.Success(
                    await _dichVuYTeService.UpdateAsync(DichVuYTeDTO), $"Cập nhật dịch vụ y tế với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _dichVuYTeService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DichVuYTeDTO>.Fail(ex.Message));
            }
        }
    }
}
