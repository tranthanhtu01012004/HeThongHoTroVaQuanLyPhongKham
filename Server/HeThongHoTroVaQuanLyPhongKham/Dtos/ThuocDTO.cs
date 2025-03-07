using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class ThuocDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã thuốc phải là số dương")]
        public int MaThuoc { get; set; }

        [Required(ErrorMessage = "Tên thuốc là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tên thuốc phải từ 1 đến 100 ký tự")]
        public string Ten { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Mô tả phải từ 1 đến 500 ký tự")]
        public string MoTa { get; set; } = null!;
    }
}
