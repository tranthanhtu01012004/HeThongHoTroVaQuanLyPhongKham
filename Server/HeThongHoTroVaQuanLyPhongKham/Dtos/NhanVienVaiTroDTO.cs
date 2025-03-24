using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class NhanVienVaiTroDTO
    {
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Mã vai trò là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã vai trò phải là số dương")]
        public int MaVaiTro { get; set; }

        [StringLength(50, ErrorMessage = "Ca làm việc không được vượt quá 50 ký tự")]
        public string? CaLamViec { get; set; }

        [StringLength(100, ErrorMessage = "Chuyên môn không được vượt quá 100 ký tự")]
        public string? ChuyenMon { get; set; }
    }
}
