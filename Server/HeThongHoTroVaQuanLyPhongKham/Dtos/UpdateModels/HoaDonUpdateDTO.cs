using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class HoaDonUpdateDTO
    {
        public int MaHoaDon { get; set; }

        [Range(0, 9999999999.99, ErrorMessage = "Tổng tiền phải từ 0 đến 9,999,999,999.99")]
        public decimal? TongTien { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Trạng thái thanh toán phải từ 1 đến 50 ký tự")]
        [RegularExpression(@"^(Chưa thanh toán|Đã thanh toán)$", ErrorMessage = "Trạng thái thanh toán chỉ được là 'Chưa thanh toán' hoặc 'Đã thanh toán'")]
        public string? TrangThaiThanhToan { get; set; }
    }
}
