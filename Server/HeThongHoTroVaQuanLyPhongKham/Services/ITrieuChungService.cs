using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface ITrieuChungService : IService<TrieuChungDTO>
    {
        Task DeleteByMaHoSoYTeAsync(int id);
    }
}
