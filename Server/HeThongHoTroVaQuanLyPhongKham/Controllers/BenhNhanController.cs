using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class BenhNhanController : ControllerBase
    {
        private readonly IBenhNhanService _benhNhanService;

        public BenhNhanController(IBenhNhanService benhNhanService)
        {
            _benhNhanService = benhNhanService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _benhNhanService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<BenhNhanDTO>>.Success(items, page, pageSize, totalPages, totalItems));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "QuanLy,BacSi,YTa,NhanVienHanhChinh,LeTan,BenhNhan")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<BenhNhanDTO>.Success(
                    await _benhNhanService.GetByIdAsync(id), $"Tìm thấy bệnh nhân với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("by-tai-khoan/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getBenhNhanByMaTaiKhoan([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<BenhNhanDTO>.Success(
                    await _benhNhanService.getBenhNhanByMaTaiKhoan(id)));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }
        [HttpGet("by-name")]
        [Authorize(Roles = "QuanLy,BacSi,TroLyBacSy")]
        public async Task<IActionResult> GetBenhNhanByName([FromQuery] string name)
        {
            var result = await _benhNhanService.GetBenhNhanByNameAsync(name);
            return Ok(ApiResponse<List<BenhNhanDTO>>.Success(result));
        }


        [HttpPost]
        [Authorize(Roles = "LeTan,NhanVienHanhChinh")]
        public async Task<IActionResult> Create([FromBody] BenhNhanDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = dto.MaBenhNhan },
                    ApiResponse<BenhNhanDTO>.Success(
                        await _benhNhanService.AddAsync(dto), "Thêm bệnh nhân thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "QuanLy,NhanVienHanhChinh")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BenhNhanDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaBenhNhan)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<BenhNhanDTO>.Success(
                    await _benhNhanService.UpdateAsync(dto), $"Cập nhật bệnh nhân với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }

        [HttpPatch("{id:int}/name")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateForPatient([FromRoute] int id, [FromBody] BenhNhanUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaBenhNhan)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<BenhNhanDTO>.Success(
                    await _benhNhanService.updateForTenAsync(dto), $"Cập nhật bệnh nhân với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "QuanLy,NhanVienHanhChinh")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _benhNhanService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BenhNhanDTO>.Fail(ex.Message));
            }
        }
    }
}
