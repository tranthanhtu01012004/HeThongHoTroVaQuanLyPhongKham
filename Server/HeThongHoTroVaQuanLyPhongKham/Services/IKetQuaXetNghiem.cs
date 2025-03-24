using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IKetQuaXetNghiem : IService<KetQuaXetNghiemDTO>
    {
        Task DeleteByMaHoSoYTeAsync(int id);
    }
}
