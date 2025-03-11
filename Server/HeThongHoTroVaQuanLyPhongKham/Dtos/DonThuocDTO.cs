using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocDTO
    {
        public int MaDonThuoc { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã hồ sơ y tế  phải là số dương")]
        public int MaHoSoYte { get; set; }

        [Required(ErrorMessage = "Ngày kê đơn là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày kê đơn không đúng định dạng")]
        public DateTime NgayKeDon { get; set; }

        public List<DonThuocChiTietDTO> ChiTietThuocList { get; set; } = new List<DonThuocChiTietDTO>();
    }
}
