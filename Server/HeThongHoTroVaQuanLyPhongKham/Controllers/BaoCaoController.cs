using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [AllowAnonymous]
    public class BaoCaoController : ControllerBase
    {
        private readonly IBaoCaoService _baoCaoService;

        public BaoCaoController(IBaoCaoService baoCaoService)
        {
            _baoCaoService = baoCaoService;
        }

        [HttpGet("revenues")]
        public async Task<ActionResult<DoanhThuReportDTO>> ThongKeDoanhThu(
            [FromQuery] DateTime tuNgay,
            [FromQuery] DateTime denNgay,
            [FromQuery] string trangThaiThanhToan = null)
        {
            var report = await _baoCaoService.ThongKeDoanhThuAsync(tuNgay, denNgay, trangThaiThanhToan);
            return Ok(report);
        }

        [HttpGet("appointments")]
        public async Task<ActionResult<LichHenReportDTO>> ThongKeLichHen(
            [FromQuery] DateTime tuNgay,
            [FromQuery] DateTime denNgay)
        {
            var report = await _baoCaoService.ThongKeLichHenAsync(tuNgay, denNgay);
            return Ok(report);
        }

        [HttpGet("prescriptions")]
        public async Task<ActionResult<DonThuocReportDTO>> ThongKeDonThuoc(
            [FromQuery] DateTime tuNgay,
            [FromQuery] DateTime denNgay)
        {
            var report = await _baoCaoService.ThongKeDonThuocAsync(tuNgay, denNgay);
            return Ok(report);
        }

        [HttpGet("healthcare-services")]
        public async Task<ActionResult<DichVuYTeReportDTO>> ThongKeDichVuYTe(
            [FromQuery] DateTime tuNgay,
            [FromQuery] DateTime denNgay)
        {
            var report = await _baoCaoService.ThongKeDichVuYTeAsync(tuNgay, denNgay);
            return Ok(report);
        }
    }
}
