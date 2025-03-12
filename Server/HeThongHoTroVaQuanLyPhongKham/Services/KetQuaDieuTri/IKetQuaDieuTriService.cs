using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri
{
    public interface IKetQuaDieuTriService : IService<KetQuaDieuTriDTO>
    {
        Task DeleteByMaHoSoYTeAsync(int id);
    }
}
