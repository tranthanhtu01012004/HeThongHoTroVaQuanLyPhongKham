namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocReportDTO
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public Dictionary<int, int> SoLuongTheoBenhNhan { get; set; } = new Dictionary<int, int>(); // Key: MaBenhNhan, Value: Số lượng đơn thuốc
        public int TongSoDonThuoc { get; set; }
    }
}
