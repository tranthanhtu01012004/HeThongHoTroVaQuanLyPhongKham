using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface ITaiKhoanService : IService<TaiKhoanDTO>
    {
        Task<TaiKhoanDTO> UpdateAsync(TaiKhoanUpdateDTO dto);
        Task<TaiKhoanDTO> AddNhanVienAsync(TaiKhoanDTO dto);
    }
}
