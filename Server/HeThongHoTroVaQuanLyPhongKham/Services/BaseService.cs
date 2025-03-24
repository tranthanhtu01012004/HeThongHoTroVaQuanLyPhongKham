namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public abstract class BaseService
    {
        protected int CalculatePageSkip(int page, int pageSize) => (page - 1) * pageSize;
        protected int CalculateTotalPages(int totalItems, int pageSize) => (int)Math.Ceiling((double)totalItems / pageSize);
        protected DateTime CalculateCurrentTime() => DateTime.Now;
    }
}
