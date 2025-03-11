using System.ComponentModel.DataAnnotations;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class TrieuChungDTO
    {
        public int MaTrieuChung { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế phải là số dương")]
        public int MaHoSoYte { get; set; }

        [Required(ErrorMessage = "Tên triệu chứng là bắt buộc")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Tên triệu chứng phải từ 1 đến 200 ký tự")]
        public string TenTrieuChung { get; set; } = null!;

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Mô tả phải từ 1 đến 500 ký tự")]
        public string? MoTa { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Thời gian xuất hiện không đúng định dạng")]
        public DateTime? ThoiGianXuatHien { get; set; }

    }
}
