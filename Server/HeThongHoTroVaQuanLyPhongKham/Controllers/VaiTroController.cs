using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/roles")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroService _vaiTroService;

        public VaiTroController(IVaiTroService vaiTroService)
        {
            _vaiTroService = vaiTroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<VaiTroDTO>>.Success(await _vaiTroService.GetAllAsync()));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<VaiTroDTO>.Success(
                    await _vaiTroService.GetByIdAsync(id), $"Tìm thấy vai trò với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VaiTroDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaVaiTro },
                    ApiResponse<VaiTroDTO>.Success(
                        await _vaiTroService.AddAsync(dto), "Thêm vai trò thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] VaiTroDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaVaiTro)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<VaiTroDTO>.Success(
                    await _vaiTroService.UpdateAsync(dto), $"Cập nhật vai trò với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _vaiTroService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<VaiTroDTO>.Fail(ex.Message));
            }
        }
    }
}
