using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class HoaDonMapper : IMapper<HoaDonDTO, TblHoaDon>
    {
        public TblHoaDon MapDtoToEntity(HoaDonDTO dto)
        {
            return new TblHoaDon
            {
                MaHoaDon = dto.MaHoaDon,
                MaLichHen = dto.MaLichHen,
                TongTien = dto.TongTien,
                NgayThanhToan = dto.NgayThanhToan,
                TrangThaiThanhToan = dto.TrangThaiThanhToan
            };
        }

        public void MapDtoToEntity(HoaDonDTO dto, TblHoaDon entity)
        {
            entity.MaHoaDon = dto.MaHoaDon;
            entity.MaLichHen = dto.MaLichHen;
            entity.TongTien = dto.TongTien;
            entity.NgayThanhToan = dto.NgayThanhToan;
            entity.TrangThaiThanhToan = dto.TrangThaiThanhToan;
        }

        public HoaDonDTO MapEntityToDto(TblHoaDon entity)
        {
            return new HoaDonDTO
            {
                MaHoaDon = entity.MaHoaDon,
                MaLichHen = entity.MaLichHen,
                TongTien = entity.TongTien,
                NgayThanhToan = entity.NgayThanhToan,
                TrangThaiThanhToan = entity.TrangThaiThanhToan
            };
        }
    }
}
