using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class KetQuaXetNghiemDTO
    {
        public int MaKetQua { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế là số dương")]
        public int MaHoSoYte { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Tên xét nghiệm phải từ 1 đến 200 ký tự")]
        public string TenXetNghiem { get; set; } = null!;

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Kết quả phải từ 1 đến 500 ký tự")]
        public string? KetQua { get; set; }

        [Required(ErrorMessage = "Ngày xét nghiệm là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày xét nghiệm không đúng định dạng")]
        public DateTime NgayXetNghiem { get; set; }
    }
}
