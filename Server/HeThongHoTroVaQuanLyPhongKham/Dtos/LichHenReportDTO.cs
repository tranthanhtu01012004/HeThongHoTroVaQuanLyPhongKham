namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class LichHenReportDTO
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Dictionary<string, int> SoLuongTheoTrangThai { get; set; } = new Dictionary<string, int>();
        public int TongSoLichHen { get; set; }
    }
}
