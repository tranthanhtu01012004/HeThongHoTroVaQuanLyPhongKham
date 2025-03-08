namespace HeThongHoTroVaQuanLyPhongKham.Common
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
