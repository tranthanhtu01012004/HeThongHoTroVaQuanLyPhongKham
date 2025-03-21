using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class HoSoYTeDTO
    {
        public int MaHoSoYTe { get; set; }

        [Required(ErrorMessage = "Mã bệnh nhân là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã bệnh nhân phải là số dương")]
        public int MaBenhNhan { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "Chẩn đoán tối đa 500 ký tự")]
        public string? ChuanDoan { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "Phương pháp điều trị tối đa 500 ký tự")]
        public string? PhuongPhapDieuTri { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "Lịch sử bệnh tối đa 500 ký tự")]
        public string? LichSuBenh { get; set; }
    }
}
