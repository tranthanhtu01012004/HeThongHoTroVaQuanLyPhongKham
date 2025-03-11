using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/treatment-outcomes")]
    [ApiController]
    public class KetQuaDieuTriController : ControllerBase
    {
        private readonly IService<KetQuaDieuTriDTO> _ketQuaDieuTriSerice;

        public KetQuaDieuTriController(IService<KetQuaDieuTriDTO> ketQuaDieuTriSerice)
        {
            _ketQuaDieuTriSerice = ketQuaDieuTriSerice;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<KetQuaDieuTriDTO>>.Success(
                    await _ketQuaDieuTriSerice.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,BenhNhan")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<KetQuaDieuTriDTO>.Success(
                    await _ketQuaDieuTriSerice.GetByIdAsync(id), $"Tìm thấy kết quả điều trị với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Create([FromBody] KetQuaDieuTriDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaKetQuaDieuTri },
                    ApiResponse<KetQuaDieuTriDTO>.Success(
                        await _ketQuaDieuTriSerice.AddAsync(dto), "Thêm kết quả điều trị thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] KetQuaDieuTriDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaKetQuaDieuTri)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<KetQuaDieuTriDTO>.Success(
                    await _ketQuaDieuTriSerice.UpdateAsync(dto), $"Cập nhật kết quả điều trị với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _ketQuaDieuTriSerice.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<KetQuaDieuTriDTO>.Fail(ex.Message));
            }
        }
    }
}
