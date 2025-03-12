using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IDonThuocService : IService<DonThuocDTO>
    {
        Task DeleteByMaHoSoYTeAsync(int id);
    }
}
