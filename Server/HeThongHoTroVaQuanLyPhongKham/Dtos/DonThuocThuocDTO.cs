using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocThuocDTO
    {
        [Required(ErrorMessage = "Mã đơn thuốc là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã đơn thuốc phải là số dương")]
        public int MaDonThuoc { get; set; }

        [Required(ErrorMessage = "Mã thuốc là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã thuốc phải là số dương")]
        public int MaThuoc { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 1000000, ErrorMessage = "Số lượng phải từ 1 đến 1,000,000")]
        public int SoLuong { get; set; }
    }
}
