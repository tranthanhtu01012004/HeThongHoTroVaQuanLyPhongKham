using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Services;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/clinic-employees")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class PhongKhamNhanVienController : ControllerBase
    {
        private readonly IService<PhongKhamNhanVienDTO> _phongKhamNhanVienService;

        public PhongKhamNhanVienController(IService<PhongKhamNhanVienDTO> phongKhamNhanVienService)
        {
            _phongKhamNhanVienService = phongKhamNhanVienService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalItems, totalPages) = await _phongKhamNhanVienService.GetAllAsync(page, pageSize);
                return Ok(ApiResponse<IEnumerable<PhongKhamNhanVienDTO>>.Success(items, page, pageSize, totalPages, totalItems));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<PhongKhamNhanVienDTO>.Success(
                    await _phongKhamNhanVienService.GetByIdAsync(id), $"Tìm thấy PhongKhamNhanVien với ID [{id}]."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhongKhamNhanVienDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(ApiResponse<PhongKhamNhanVienDTO>.Success(
                    await _phongKhamNhanVienService.AddAsync(dto), "Thêm dữ liệu cho PhongKhamNhanVien thành công."));

            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PhongKhamNhanVienDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != dto.MaPhongKhamNhanVien)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<PhongKhamNhanVienDTO>.Success(
                    await _phongKhamNhanVienService.UpdateAsync(dto), $"Cập nhật PhongKhamNhanVien với ID [{id}] thành công."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _phongKhamNhanVienService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            }
        }
    }
}
