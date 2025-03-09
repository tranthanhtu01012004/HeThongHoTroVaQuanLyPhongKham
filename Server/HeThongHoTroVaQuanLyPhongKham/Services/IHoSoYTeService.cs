using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IHoSoYTeService : IService<HoSoYTeDTO>
    {
        Task<HoSoYTeDTO> UpdateChuanDoanAsync(HoSoYTeUpdateDTO dto);
        Task<HoSoYTeDTO> UpdatePhuongPhapDieuTriAsync(HoSoYTeUpdateDTO dto);
    }
}
