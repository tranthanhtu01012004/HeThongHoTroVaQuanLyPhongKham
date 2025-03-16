using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IPhongKhamService : IService<PhongKhamDTO>
    {
        Task<IEnumerable<PhongKhamDTO>> GetAllAsync();
    }
}
