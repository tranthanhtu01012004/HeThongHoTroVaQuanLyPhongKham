using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IThuocService : IService<ThuocDTO>
    {
        Task<IEnumerable<ThuocDTO>> GetAllAsync();
    }
}
