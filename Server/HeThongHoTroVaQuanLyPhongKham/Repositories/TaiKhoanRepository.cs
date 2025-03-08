using HeThongHoTroVaQuanLyPhongKham.Data;
using HeThongHoTroVaQuanLyPhongKham.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Repositories
{
    public class TaiKhoanRepository : Repository<TblTaiKhoan>, ITaiKhoanRepository
    {
        public TaiKhoanRepository(ApplicationDbContext context, ILogger<TblTaiKhoan> logger) : base(context, logger)
        {
        }

        public async Task<TblTaiKhoan> FindByNameAsync(string tenDangNhap)
        {
            return await _context.TblTaiKhoans
                .Include(tk => tk.MaVaiTroNavigation)
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == tenDangNhap);
        }

        public async Task<TblTaiKhoan> FindByIdAsync(int id)
        {
            return await _context.TblTaiKhoans
                .Include(tk => tk.MaVaiTroNavigation)
                .FirstOrDefaultAsync(tk => tk.MaTaiKhoan == id);
        }
    }
}
