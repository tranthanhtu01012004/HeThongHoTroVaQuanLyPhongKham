using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class TaiKhoanUpdateDTO
    {
        public int MaTaiKhoan { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Mã vai trò phải là số dương nếu có")]
        public int? MaVaiTro { get; set; }

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 255 ký tự")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái và một số")]
        public string? MatKhau { get; set; }
    }
}
