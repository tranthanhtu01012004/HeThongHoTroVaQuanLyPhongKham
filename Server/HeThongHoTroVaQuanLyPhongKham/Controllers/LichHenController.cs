using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/appointments")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class LichHenController : ControllerBase
    {
        private readonly ILichHenService _lichHenService;

        public LichHenController(ILichHenService lichHenService)
        {
            _lichHenService = lichHenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? ngayHen = null,
            [FromQuery] int? maNhanVien = null,
            [FromQuery] int? maPhong = null)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<LichHenDTO>>.Success(await _lichHenService.GetAllAsync(page, pageSize, ngayHen, maNhanVien, maPhong), $"Đã lấy danh sách lịch hẹn - trang {page} với {pageSize} bản ghi."));
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

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateTrangThai([FromRoute] int id, [FromBody] LichHenUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaLichHen)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<LichHenDTO>.Success(
                    await _lichHenService.UpdateTrangThaiAsync(dto), $"Cập nhật lịch hẹn với mã lịch hẹn [{id}] - trạng thái [{dto.TrangThai}] thành công"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
               return Ok(ApiResponse<LichHenDTO>.Success(await _lichHenService.GetByIdAsync(id), $"Tìm thấy lịch hẹn với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LichHenDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = dto.MaLichHen }, 
                    ApiResponse<LichHenDTO>.Success(
                        await _lichHenService.AddAsync(dto), "Thêm lịch hẹn thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] LichHenDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != dto.MaLichHen)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<LichHenDTO>.Success(
                    await _lichHenService.UpdateAsync(dto), $"Cập nhật lịch hẹn với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _lichHenService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LichHenDTO>.Fail(ex.Message));
            }
        }
    }
}
