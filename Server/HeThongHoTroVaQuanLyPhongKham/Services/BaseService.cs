namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public abstract class BaseService
    {
        protected int CalculatePageSkip(int page, int pageSize) => (page - 1) * pageSize;
    }
}
