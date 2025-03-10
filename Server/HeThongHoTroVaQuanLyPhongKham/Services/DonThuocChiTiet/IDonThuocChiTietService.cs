using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet
{
    public interface IDonThuocChiTietService : IService<DonThuocChiTietDTO>
    {
        Task AddAsync(IEnumerable<DonThuocChiTietDTO> dto);
    }
}
