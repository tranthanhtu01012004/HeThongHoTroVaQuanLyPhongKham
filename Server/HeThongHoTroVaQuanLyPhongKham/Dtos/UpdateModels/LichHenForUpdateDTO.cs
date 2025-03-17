using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class LichHenForUpdateDTO
    {
        public int MaLichHen { get; set; }

        [Required(ErrorMessage = "Nhân viên là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã nhân viên phải là số dương")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Phòng khám là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Phòng khám phải là số dương")]
        public int MaPhongKham { get; set; }
    }
}
