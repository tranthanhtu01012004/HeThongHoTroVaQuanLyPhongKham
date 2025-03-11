using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DonThuocDTO
    {
        public int MaDonThuoc { get; set; }

        [Required(ErrorMessage = "Ngày kê đơn là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày kê đơn không đúng định dạng")]
        public DateTime NgayKeDon { get; set; }

        public List<DonThuocChiTietDTO> ChiTietThuocList { get; set; } = new List<DonThuocChiTietDTO>();
    }
}
