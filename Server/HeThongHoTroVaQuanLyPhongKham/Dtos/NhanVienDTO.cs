using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class NhanVienDTO
    {
        public int MaNhanVien { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Mã tài khoản phải là số dương nếu có")]
        public int? MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tên nhân viên phải từ 1 đến 100 ký tự")]
        public string Ten { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số")]
        public string SoDienThoai { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ca làm việc phải từ 3 đến 50 ký tự")]
        public string? CaLamViec { get; set; }

        [Required(ErrorMessage = "Chuyên môn là bắt buộc")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Chuyên môn phải từ 3 đến 100 ký tự")]
        public string ChuyenMon { get; set; } = null!;
    }
}
