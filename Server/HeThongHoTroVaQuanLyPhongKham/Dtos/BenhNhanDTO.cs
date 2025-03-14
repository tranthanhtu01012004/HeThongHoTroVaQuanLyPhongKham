using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class BenhNhanDTO
    {
        public int MaBenhNhan { get; set; }
        public int MaTaiKhoan { get; set; }

        [Range(0, 150, ErrorMessage = "Tuổi phải từ 0 đến 150")]
        public int? Tuoi { get; set; }
        public bool? GioiTinh { get; set; }

        [StringLength(1000, ErrorMessage = "Địa chỉ không được vượt quá 1000 ký tự")]
        public string? DiaChi { get; set; }

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự")]
        [RegularExpression(@"^0\d{9,14}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và chứa 10-15 chữ số")]
        public string? SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string? Email { get; set; }
    }
}
