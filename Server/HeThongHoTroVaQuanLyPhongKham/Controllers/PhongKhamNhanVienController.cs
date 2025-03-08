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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhongKhamNhanVienDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(ApiResponse<PhongKhamNhanVienDTO>.Success(
                    await _phongKhamNhanVienService.AddAsync(dto), "Thêm dữ liệu cho PhongKhamNhanVien thành công."));

            } catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<PhongKhamNhanVienDTO>.Fail(ex.Message));
            } catch (Exception ex)
            {
                return BadRequest(ApiResponse<NhanVienDTO>.Fail(ex.Message));
            }
        }
    }
}
