using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class HoSoYTeDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế phải là số dương")]
        public int MaHoSoYTe { get; set; }

        [Required(ErrorMessage = "Mã bệnh nhân là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã bệnh nhân phải là số dương")]
        public int MaBenhNhan { get; set; }

        [Required(ErrorMessage = "Chẩn đoán là bắt buộc")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Chẩn đoán phải từ 1 đến 500 ký tự")]
        public string ChuanDoan { get; set; } = null!;

        [Required(ErrorMessage = "Phương pháp điều trị là bắt buộc")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Phương pháp điều trị phải từ 1 đến 500 ký tự")]
        public string PhuongPhapDieuTri { get; set; } = null!;
    }
}
