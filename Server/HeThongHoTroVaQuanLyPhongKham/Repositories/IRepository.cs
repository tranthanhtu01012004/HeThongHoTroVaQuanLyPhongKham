namespace HeThongHoTroVaQuanLyPhongKham.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync(int page, int pageSize, int pageSkip);
        Task<T> FindByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
