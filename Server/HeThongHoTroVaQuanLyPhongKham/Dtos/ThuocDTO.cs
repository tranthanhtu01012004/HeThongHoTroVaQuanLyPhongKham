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

        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Đơn vị phải từ 1 đến 20 ký tự")]
        public string DonVi { get; set; } = null!;

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Chống chỉ định phải từ 1 đến 1000 ký tự")]
        public string? ChongChiDinh { get; set; }

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Tương tác phải từ 1 đến 1000 ký tự")]
        public string? TuongTacThuoc { get; set; }

    }
}
