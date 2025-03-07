using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã đơn thuốc phải là số dương")]
        public int MaDonThuoc { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế phải là số dương")]
        public int MaHoSoYTe { get; set; }

        [Required(ErrorMessage = "Liều lượng là bắt buộc")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Liều lượng phải từ 1 đến 500 ký tự")]
        public string LieuLuong { get; set; } = null!;
    }
}
