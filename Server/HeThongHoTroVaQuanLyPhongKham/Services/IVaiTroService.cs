using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IVaiTroService : IService<VaiTroDTO>
    {
        Task<IEnumerable<VaiTroDTO>> GetAllAsync();
    }
}
