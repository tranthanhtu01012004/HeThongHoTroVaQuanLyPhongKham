using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HeThongHoTroVaQuanLyPhongKham.Services;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/medical-records")]
    [ApiController]
    public class HoSoYTeController : ControllerBase
    {
        private readonly IHoSoYTeService _hoSoYTeService;

        public HoSoYTeController(IHoSoYTeService hoSoYTeService)
        {
            _hoSoYTeService = hoSoYTeService;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,YTa,TroLyBacSy,NhanVienHanhChinh")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<HoSoYTeDTO>>.Success(
                    await _hoSoYTeService.GetAllAsync(page, pageSize)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,YTa,TroLyBacSy,NhanVienHanhChinh,BenhNhan")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<HoSoYTeDTO>.Success(
                    await _hoSoYTeService.GetByIdAsync(id), $"Tìm thấy hồ sơ với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> Create([FromBody] HoSoYTeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaHoSoYTe },
                    ApiResponse<HoSoYTeDTO>.Success(
                        await _hoSoYTeService.AddAsync(dto), "Thêm hồ sơ thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] HoSoYTeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaHoSoYTe)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<HoSoYTeDTO>.Success(
                    await _hoSoYTeService.UpdateAsync(dto), $"Cập nhật hồ sơ với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPatch("{id:int}/diagnosis")]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> UpdateChuanDoan([FromRoute] int id, [FromBody] HoSoYTeUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaHoSoYTe)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<HoSoYTeDTO>.Success(
                    await _hoSoYTeService.UpdateChuanDoanAsync(dto), $"Cập nhật hồ sơ với ID [{id}] - chuẩn đoán [{dto.ChuanDoan}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpPatch("{id:int}/treatment-method")]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> UpdatePhuongPhapDieuTri([FromRoute] int id, [FromBody] HoSoYTeUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaHoSoYTe)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<HoSoYTeDTO>.Success(
                    await _hoSoYTeService.UpdatePhuongPhapDieuTriAsync(dto), $"Cập nhật hồ sơ với ID [{id}] - Phương pháp điều trị [{dto.PhuongPhapDieuTri}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _hoSoYTeService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<HoSoYTeDTO>.Fail(ex.Message));
            }
        }
    }
}
