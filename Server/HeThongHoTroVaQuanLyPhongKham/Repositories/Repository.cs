using HeThongHoTroVaQuanLyPhongKham.Data;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<T> _logger;

        public Repository(ApplicationDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Đã tạo {typeof(T).Name} thành công.");
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Đã xóa {typeof(T).Name} thành công.");
        }

        public async Task<IEnumerable<T>> FindAllAsync(int page, int pageSize, int pageSkip)
        {
            var entities = await _context.Set<T>()
                .OrderBy(e => EF.Property<int>(e, $"Ma{typeof(T).Name}"))
                .Skip(pageSkip)
                .Take(pageSize)
                .ToListAsync();
            _logger.LogInformation($"Đã lấy danh sách {typeof(T).Name} - trang {page} với {pageSize} bản ghi.");
            return entities;
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, $"Ma{typeof(T).Name}") == id);
            _logger.LogInformation($"Đã tìm thấy {typeof(T).Name} với ID {id}.");
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Đã cập nhật {typeof(T).Name} thành công.");
            return entity;
        }
    }
}
