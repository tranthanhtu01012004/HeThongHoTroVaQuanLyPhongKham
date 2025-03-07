using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class DichVuYTeDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Mã dịch vụ y tế phải là số dương")]
        public int MaDichVuYTe { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Tên dịch vụ phải từ 1 đến 100 ký tự")]
        public string Ten { get; set; } = null!;

        [Required(ErrorMessage = "Chi phí là bắt buộc")]
        [Range(0, 9999999999.99, ErrorMessage = "Chi phí phải từ 0 đến 9,999,999,999.99")]
        public decimal ChiPhi { get; set; }
    }
}
