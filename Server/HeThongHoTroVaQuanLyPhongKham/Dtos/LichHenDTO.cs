using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    namespace HeThongHoTroVaQuanLyPhongKham.DTOs
    {
        public class LichHenDTO
        {
            public int MaLichHen { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Mã bệnh nhân phải là số dương")]
            public int MaBenhNhan { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Mã nhân viên phải là số dương")]
            public int? MaNhanVien { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Mã dịch vụ y tế phải là số dương")]
            public int? MaDichVuYTe { get; set; }

            public int? MaPhongKham { get; set; }

            [Required(ErrorMessage = "Ngày hẹn là bắt buộc")]
            [DataType(DataType.DateTime, ErrorMessage = "Ngày hẹn không đúng định dạng")]
            public DateTime NgayHen { get; set; }

            [StringLength(50, MinimumLength = 1, ErrorMessage = "Trạng thái phải từ 1 đến 50 ký tự")]
            [RegularExpression(@"^(Chờ xác nhận|Đã xác nhận|Đã hoàn thành|Hủy)$", ErrorMessage = "Trạng thái chỉ được là 'Chờ xác nhận', 'Đã xác nhận', 'Đã hoàn thành' hoặc 'Hủy'")]
            public string TrangThai { get; set; } = null!;
        }
    }
}
