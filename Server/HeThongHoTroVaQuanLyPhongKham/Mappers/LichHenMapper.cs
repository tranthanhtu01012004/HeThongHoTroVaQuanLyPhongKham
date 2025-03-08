using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class LichHenMapper : IMapper<LichHenDTO, TblLichHen>
    {
        public TblLichHen MapDtoToEntity(LichHenDTO dto)
        {
            return new TblLichHen
            {
                MaLichHen = dto.MaLichHen,
                MaBenhNhan = dto.MaBenhNhan,
                MaNhanVien = dto.MaNhanVien,
                MaDichVuYte = dto.MaDichVuYTe,
                MaPhongKham = dto.MaPhongKham,
                NgayHen = dto.NgayHen,
                TrangThai = dto.TrangThai
            };
        }

        public void MapDtoToEntity(LichHenDTO dto, TblLichHen entity)
        {
            entity.MaLichHen = dto.MaLichHen;
            entity.MaBenhNhan = dto.MaBenhNhan;
            entity.MaNhanVien = dto.MaNhanVien;
            entity.MaDichVuYte = dto.MaDichVuYTe;
            entity.MaPhongKham = dto.MaPhongKham;
            entity.NgayHen = dto.NgayHen;
            entity.TrangThai = dto.TrangThai;
        }

        public LichHenDTO MapEntityToDto(TblLichHen entity)
        {
            return new LichHenDTO
            {
                MaLichHen = entity.MaLichHen,
                MaBenhNhan = entity.MaBenhNhan,
                MaNhanVien = entity.MaNhanVien,
                MaDichVuYTe = entity.MaDichVuYte,
                MaPhongKham = entity.MaPhongKham,
                NgayHen = entity.NgayHen,
                TrangThai = entity.TrangThai
            };
        }
    }
}
