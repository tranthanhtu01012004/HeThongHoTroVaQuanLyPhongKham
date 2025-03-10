using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocChiTietDTO
    {
        public int MaDonThuoc { get; set; }
        public int MaThuoc { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 1000000, ErrorMessage = "Số lượng phải từ 1 đến 1,000,000")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Cách dùng là bắt buộc")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Cách dùng phải từ 1 đến 200 ký tự")]
        public string CachDung { get; set; } = null!;
    }
}
