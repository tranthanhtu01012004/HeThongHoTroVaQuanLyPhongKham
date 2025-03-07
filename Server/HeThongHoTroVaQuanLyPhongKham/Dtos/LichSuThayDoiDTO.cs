using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class LichSuThayDoiDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã lịch sử phải là số dương")]
        public int MaLichSu { get; set; }

        [Required(ErrorMessage = "Mã nhân viên là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã nhân viên phải là số dương")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Mã bản ghi là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã bản ghi phải là số dương")]
        public int MaBanGhi { get; set; }

        [Required(ErrorMessage = "Thời gian là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Thời gian không đúng định dạng")]
        public DateTime ThoiGian { get; set; }

        [Required(ErrorMessage = "Bảng liên quan là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Bảng liên quan phải từ 1 đến 100 ký tự")]
        public string BangLienQuan { get; set; } = null!;

        [Required(ErrorMessage = "Hành động là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Hành động phải từ 1 đến 100 ký tự")]
        public string HanhDong { get; set; } = null!;
    }
}
