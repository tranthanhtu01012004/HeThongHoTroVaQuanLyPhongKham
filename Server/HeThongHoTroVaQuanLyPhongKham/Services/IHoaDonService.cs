using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IHoaDonService : IService<HoaDonDTO>
    {
        Task<HoaDonDTO> UpdateTrangThaiAsync(HoaDonUpdateDTO dto);
        Task<HoaDonDTO> UpdateTongTienAsync(HoaDonUpdateDTO dto);
    }
}
