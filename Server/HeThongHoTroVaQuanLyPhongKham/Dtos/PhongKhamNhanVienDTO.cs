using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class PhongKhamNhanVienDTO
    {
        [Required(ErrorMessage = "Mã phòng khám là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã phòng khám phải là số dương")]
        public int MaPhongKham { get; set; }

        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã nhân viên phải là số dương")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Vai trò phải từ 1 đến 50 ký tự")]
        public string VaiTro { get; set; } = null!;
    }
}
