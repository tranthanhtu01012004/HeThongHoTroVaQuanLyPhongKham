using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class PhongKhamDTO
    {
        public int MaPhongKham { get; set; }

        [Required(ErrorMessage = "Loại phòng khám là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Loại phòng khám phải từ 1 đến 100 ký tự")]
        public string Loai { get; set; } = null!;

        [Required(ErrorMessage = "Sức chứa là bắt buộc")]
        [Range(1, 1000, ErrorMessage = "Sức chứa phải từ 1 đến 1000")]
        public int SucChua { get; set; }
    }
}
