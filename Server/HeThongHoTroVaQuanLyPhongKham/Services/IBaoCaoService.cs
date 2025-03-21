using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IBaoCaoService
    {
        Task<DoanhThuReportDTO> ThongKeDoanhThuAsync(DateTime tuNgay, DateTime denNgay, string trangThaiThanhToan = null);
        Task<LichHenReportDTO> ThongKeLichHenAsync(DateTime tuNgay, DateTime denNgay);
        Task<DonThuocReportDTO> ThongKeDonThuocAsync(DateTime tuNgay, DateTime denNgay);
        Task<DichVuYTeReportDTO> ThongKeDichVuYTeAsync(DateTime tuNgay, DateTime denNgay);
    }
}
