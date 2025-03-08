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
    [Authorize(Roles = "QuanLy")]
    public class ThuocController : ControllerBase
    {
        private readonly IService<ThuocDTO> _thuocService;

        public ThuocController(IService<ThuocDTO> thuocService)
        {
            _thuocService = thuocService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<ThuocDTO>>.Success(
                    await _thuocService.GetAllAsync(page, pageSize)));
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

        [HttpGet("{id}")]
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
        public async Task<IActionResult> Create([FromBody] ThuocDTO ThuocDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var nhanVien = await _thuocService.AddAsync(ThuocDTO);

                return Ok(ApiResponse<ThuocDTO>.Success(nhanVien, "Thêm thuốc mới thành công."));
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
