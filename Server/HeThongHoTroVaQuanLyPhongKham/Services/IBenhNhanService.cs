using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IBenhNhanService : IService<BenhNhanDTO>
    {
        Task<BenhNhanDTO> updateForTenAsync(BenhNhanUpdateDTO dto);
        Task<BenhNhanDTO> getBenhNhanByMaTaiKhoan(int id);
    }
}
