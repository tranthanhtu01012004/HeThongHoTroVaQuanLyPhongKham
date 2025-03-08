using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IAuthService
    {
        Task<TaiKhoanDTO> GetUserWithRole(TaiKhoanDTO taiKhoanDTO);
        Task<LoginResponse> Login(TaiKhoanDTO taiKhoanDTO);
    }
}
