namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = string.Empty;
    }
}
