using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Repositories
{
    public interface ITaiKhoanRepository : IRepository<TblTaiKhoan>
    {
        Task<TblTaiKhoan> FindByNameAsync(string tenDangNhap);
        Task<TblTaiKhoan> FindByIdAsync(int id);
    }
}
