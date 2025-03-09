using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class HoSoYTeUpdateDTO
    {
        public int MaHoSoYTe { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Chẩn đoán phải từ 1 đến 500 ký tự")]
        public string? ChuanDoan { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Phương pháp điều trị phải từ 1 đến 500 ký tự")]
        public string? PhuongPhapDieuTri { get; set; }
    }
}
