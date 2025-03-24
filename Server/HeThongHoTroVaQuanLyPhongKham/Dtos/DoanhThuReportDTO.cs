namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DoanhThuReportDTO
    {
        public decimal TongDoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public string TrangThaiThanhToan { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public List<HoaDonDTO> DanhSachHoaDon { get; set; } = new List<HoaDonDTO>();
    }
}
