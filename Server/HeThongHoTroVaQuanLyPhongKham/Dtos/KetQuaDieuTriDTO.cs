using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class KetQuaDieuTriDTO
    {
        public int MaKetQuaDieuTri { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế phải là số dương")]
        public int MaHoSoYte { get; set; }

        [Required(ErrorMessage = "Mã đơn thuốc là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã Mã đơn thuốc phải là số dương")]
        public int MaDonThuoc { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Hiệu quả phải từ 1 đến 500 ký tự")]
        public string? HieuQua { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "Tác dụng phụ phải từ 1 đến 500 ký tự")]
        public string? TacDungPhu { get; set; }

        [Required(ErrorMessage = "Ngày đánh giá là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày đánh giá không đúng định dạng")]
        public DateTime NgayDanhGia { get; set; }
    }
}
