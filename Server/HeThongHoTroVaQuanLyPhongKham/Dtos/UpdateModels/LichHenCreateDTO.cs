using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class LichHenCreateDTO
    {
        [Required(ErrorMessage = "Mã dịch vụ y tế là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã dịch vụ y tế phải là số dương")]
        public int MaDichVuYTe { get; set; }

        [Required(ErrorMessage = "Ngày hẹn là bắt buộc")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày hẹn không đúng định dạng")]
        public DateTime NgayHen { get; set; }
    }
}
