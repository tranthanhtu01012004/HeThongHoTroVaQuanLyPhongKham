using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class ThuocDTO
    {
        public int MaThuoc { get; set; }

        [Required(ErrorMessage = "Tên thuốc là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tên thuốc phải từ 1 đến 100 ký tự")]
        public string Ten { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Mô tả phải từ 1 đến 500 ký tự")]
        public string MoTa { get; set; } = null!;

        [Required(ErrorMessage = "Đơn vị là bắt buộc phải chọn")]
        public string DonVi { get; set; } = null!;

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Chống chỉ định phải từ 1 đến 1000 ký tự")]
        public string? ChongChiDinh { get; set; }

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Tương tác phải từ 1 đến 1000 ký tự")]
        public string? TuongTacThuoc { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, 9999999999.99, ErrorMessage = "Đơn giá phải từ 0 đến 9,999,999,999.99")]
        public decimal DonGia { get; set; }

    }
}
