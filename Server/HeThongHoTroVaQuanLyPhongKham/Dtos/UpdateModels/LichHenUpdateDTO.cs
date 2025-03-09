using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class LichHenUpdateDTO
    {
        public int MaLichHen { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Trạng thái phải từ 1 đến 50 ký tự")]
        [RegularExpression(@"^(Chờ xác nhận|Đã xác nhận|Đã hoàn thành|Hủy)$", ErrorMessage = "Trạng thái chỉ được là 'Chờ xác nhận', 'Đã xác nhận', 'Đã hoàn thành' hoặc 'Hủy'")]
        public string TrangThai { get; set; } = null!;
    }
}
