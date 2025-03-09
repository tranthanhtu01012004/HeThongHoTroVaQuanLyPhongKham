using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class HoaDonDTO
    {
        public int MaHoaDon { get; set; }

        [Required(ErrorMessage = "Mã lịch hẹn là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lịch hẹn phải là số dương")]
        public int MaLichHen { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc")]
        [Range(0, 9999999999.99, ErrorMessage = "Tổng tiền phải từ 0 đến 9,999,999,999.99")]
        public decimal TongTien { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Ngày thanh toán không đúng định dạng")]
        public DateTime? NgayThanhToan { get; set; }

        [Required(ErrorMessage = "Trạng thái thanh toán là bắt buộc")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Trạng thái thanh toán phải từ 1 đến 50 ký tự")]
        [RegularExpression(@"^(Chưa thanh toán|Đã thanh toán)$", ErrorMessage = "Trạng thái thanh toán chỉ được là 'Chưa thanh toán' hoặc 'Đã thanh toán'")]
        public string TrangThaiThanhToan { get; set; } = null!;
    }
}
