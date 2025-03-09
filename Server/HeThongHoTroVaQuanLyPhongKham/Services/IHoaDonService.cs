using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IHoaDonService : IService<HoaDonDTO>
    {
        Task<HoaDonDTO> UpdateTrangThaiAsync(int maHoaDon, HoaDonUpdateDTO dto);
        Task<HoaDonDTO> UpdateTongTienAsync(int maHoaDon, HoaDonUpdateDTO dto);
    }
}
