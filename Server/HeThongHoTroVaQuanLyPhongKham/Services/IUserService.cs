using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IUserService
    {
        Task<int> GetMaNhanVienFromTaiKhoan(int maTaiKhoan);
        Task<int> GetMaBenhNhanFromTaiKhoan(int maTaiKhoan);
    }
}
