using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface INhanVienService : IService<NhanVienDTO>
    {
        Task<IEnumerable<NhanVienDTO>> GetAllAsync();
    }
}
