using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/prescription-details")]
    [ApiController]
    public class DonThuocChiTietController : ControllerBase
    {
        private readonly IDonThuocChiTietService _donThuocChiTietService;

        public DonThuocChiTietController(IDonThuocChiTietService donThuocChiTietService)
        {
            _donThuocChiTietService = donThuocChiTietService;
        }

        [HttpGet]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi,YTa")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _donThuocChiTietService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<DonThuocChiTietDTO>>.Success(items, page, pageSize, totalPages, totalItems));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,DuocSi,YTa")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<DonThuocChiTietDTO>.Success(
                    await _donThuocChiTietService.GetByIdAsync(id), $"Tìm thấy đơn thuốc chi tiết với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DonThuocChiTietDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaDonThuocChiTiet)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<DonThuocChiTietDTO>.Success(
                    await _donThuocChiTietService.UpdateAsync(dto), $"Cập nhật đơn thuốc chi tiết với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DonThuocChiTietDTO>.Fail(ex.Message));
            }
        }
    }
}
