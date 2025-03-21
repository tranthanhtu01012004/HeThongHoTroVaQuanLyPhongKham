namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DichVuYTeReportDTO
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Dictionary<string, int> SoLuongTheoDichVu { get; set; } = new Dictionary<string, int>(); // Key: TenDichVu, Value: Số lượng bệnh nhân
        public int TongSoBenhNhan { get; set; }
    }
}
