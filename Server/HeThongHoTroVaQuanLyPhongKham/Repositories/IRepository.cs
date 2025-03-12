namespace HeThongHoTroVaQuanLyPhongKham.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync(int page, int pageSize, int pageSkip, string keyPropertyName);
        Task<T> FindByIdAsync(int id, string keyPropertyName);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
        IQueryable<T> GetQueryable();
        Task<IEnumerable<T>> FindAllWithQueryAsync(
            IQueryable<T> query,
            int page, int pageSize, 
            int pageSkip, string keyPropertyName
        );
        Task<T> FindByIdWithQueryAsync(IQueryable<T> query, int id, string keyPropertyName);
    }
}
