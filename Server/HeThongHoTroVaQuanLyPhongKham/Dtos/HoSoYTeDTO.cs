using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class HoSoYTeDTO
    {
        public int MaHoSoYTe { get; set; }

        [Required(ErrorMessage = "Mã bệnh nhân là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã bệnh nhân phải là số dương")]
        public int MaBenhNhan { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Chẩn đoán phải từ 1 đến 500 ký tự")]
        public string ChuanDoan { get; set; } = null!;

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Phương pháp điều trị phải từ 1 đến 500 ký tự")]
        public string PhuongPhapDieuTri { get; set; } = null!;

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Lịch sử bệnh phải từ 1 đến 1000 ký tự")]
        public string? LichSuBenh { get; set; }
    }
}
