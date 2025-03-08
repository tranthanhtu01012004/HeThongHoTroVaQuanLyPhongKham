namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
