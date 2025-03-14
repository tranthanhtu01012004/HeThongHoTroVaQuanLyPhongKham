using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface ILichHenService : IService<LichHenDTO>
    {
        Task<(IEnumerable<LichHenDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(
            int page, int pageSize,
            DateTime? ngayHen = null,
            int? maNhanVien = null,
            int? maPhong = null
        );
        Task<LichHenDTO> UpdateTrangThaiAsync(LichHenUpdateDTO dto);
    }
}
