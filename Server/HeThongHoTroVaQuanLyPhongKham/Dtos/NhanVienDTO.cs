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
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Tên nhân viên chỉ được chứa chữ cái và khoảng trắng")]
        public string Ten { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; } = null!;

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ca làm việc phải từ 3 đến 50 ký tự")]
        [RegularExpression(@"^(Sáng|Chiều|Toàn thời gian)$", ErrorMessage = "Ca làm việc phải là 'Sáng', 'Chiều' hoặc 'Toàn thời gian'")]
        public string? CaLamViec { get; set; }

        [Required(ErrorMessage = "Chuyên môn là bắt buộc")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Chuyên môn phải từ 3 đến 100 ký tự")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Chuyên môn chỉ được chứa chữ cái và khoảng trắng")]
        public string ChuyenMon { get; set; } = null!;

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái và một số")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Mã vai trò là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã vai trò phải là số dương")]
        public int MaVaiTro { get; set; }
    }
}
