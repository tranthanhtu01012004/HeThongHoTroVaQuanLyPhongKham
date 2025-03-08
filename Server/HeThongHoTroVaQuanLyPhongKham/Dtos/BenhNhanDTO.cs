using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class BenhNhanDTO
    {
        public int MaBenhNhan { get; set; }

        [Range(0, 150, ErrorMessage = "Tuổi phải từ 0 đến 150")]
        public int? Tuoi { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public bool GioiTinh { get; set; } // 0 = Nữ, 1 = Nam

        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số")]
        public string SoDienThoai { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
    }
}
