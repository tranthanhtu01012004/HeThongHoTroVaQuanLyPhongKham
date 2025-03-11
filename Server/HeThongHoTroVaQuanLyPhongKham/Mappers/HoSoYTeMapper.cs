using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class HoSoYTeMapper : IMapper<HoSoYTeDTO, TblHoSoYTe>
    {
        public TblHoSoYTe MapDtoToEntity(HoSoYTeDTO dto)
        {
            return new TblHoSoYTe
            {
                MaHoSoYte = dto.MaHoSoYTe,
                MaBenhNhan = dto.MaBenhNhan,
                ChuanDoan = dto.ChuanDoan,
                PhuongPhapDieuTri = dto.PhuongPhapDieuTri,
                LichSuBenh = dto.LichSuBenh
            };
        }

        public void MapDtoToEntity(HoSoYTeDTO dto, TblHoSoYTe entity)
        {
            entity.MaHoSoYte = dto.MaHoSoYTe;
            entity.MaBenhNhan = dto.MaBenhNhan;
            entity.ChuanDoan = dto.ChuanDoan;
            entity.PhuongPhapDieuTri = dto.PhuongPhapDieuTri;
            entity.LichSuBenh = dto.LichSuBenh;
        }

        public HoSoYTeDTO MapEntityToDto(TblHoSoYTe entity)
        {
            return new HoSoYTeDTO
            {
                MaHoSoYTe = entity.MaHoSoYte,
                MaBenhNhan = entity.MaBenhNhan,
                ChuanDoan = entity.ChuanDoan,
                PhuongPhapDieuTri = entity.PhuongPhapDieuTri,
                LichSuBenh = entity.LichSuBenh
            };
        }
    }
}
