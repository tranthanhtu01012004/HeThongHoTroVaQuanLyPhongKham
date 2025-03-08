using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IJwtService
    {
        string GenerateToken(TblTaiKhoan taiKhoan);
    }
}
