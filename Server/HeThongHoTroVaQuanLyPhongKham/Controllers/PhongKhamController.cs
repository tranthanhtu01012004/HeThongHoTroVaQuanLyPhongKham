using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/clinics")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class PhongKhamController : ControllerBase
    {
        private readonly IService<PhongKhamDTO> _phongKhamService;

        public PhongKhamController(IService<PhongKhamDTO> phongKhamService)
        {
            _phongKhamService = phongKhamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _phongKhamService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<PhongKhamDTO>>.Success(items, page, pageSize, totalPages, totalItems));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<PhongKhamDTO>.Success(
                    await _phongKhamService.GetByIdAsync(id), $"Tìm thấy phòng khám với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhongKhamDTO phongKhamDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = phongKhamDTO.MaPhongKham }, 
                    ApiResponse<PhongKhamDTO>.Success(
                        await _phongKhamService.AddAsync(phongKhamDTO), "Thêm phòng khám thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PhongKhamDTO PhongKhamDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != PhongKhamDTO.MaPhongKham)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<PhongKhamDTO>.Success(
                    await _phongKhamService.UpdateAsync(PhongKhamDTO), $"Cập nhật phòng khám với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _phongKhamService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamDTO>.Fail(ex.Message));
            }
        }
    }
}
