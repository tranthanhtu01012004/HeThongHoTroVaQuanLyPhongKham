namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IService<Dto> where Dto : class
    {
        Task<(IEnumerable<Dto> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize);
        Task<Dto> GetByIdAsync(int id);
        Task<Dto> AddAsync(Dto dto);
        Task<Dto> UpdateAsync(Dto dto);
        Task DeleteAsync(int id);
    }
}
