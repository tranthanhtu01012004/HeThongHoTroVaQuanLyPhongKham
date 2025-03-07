using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class VaiTroDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã vai trò phải là số dương")]
        public int MaVaiTro { get; set; }

        [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tên vai trò phải từ 1 đến 100 ký tự")]
        public string Ten { get; set; } = null!;
    }
}
