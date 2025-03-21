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
                ChuanDoan = dto.ChuanDoan ?? "chưa có",
                PhuongPhapDieuTri = dto.PhuongPhapDieuTri ?? "chưa có",
                LichSuBenh = dto.LichSuBenh ?? "không có"
            };
        }

        public void MapDtoToEntity(HoSoYTeDTO dto, TblHoSoYTe entity)
        {
            entity.MaHoSoYte = dto.MaHoSoYTe;
            entity.MaBenhNhan = dto.MaBenhNhan;
            entity.ChuanDoan = dto.ChuanDoan ?? "chưa có";
            entity.PhuongPhapDieuTri = dto.PhuongPhapDieuTri ?? "chưa có";
            entity.LichSuBenh = dto.LichSuBenh ?? "không có";
        }

        public HoSoYTeDTO MapEntityToDto(TblHoSoYTe entity)
        {
            return new HoSoYTeDTO
            {
                MaHoSoYTe = entity.MaHoSoYte,
                MaBenhNhan = entity.MaBenhNhan,
                ChuanDoan = entity.ChuanDoan ?? "chưa có",
                PhuongPhapDieuTri = entity.PhuongPhapDieuTri ?? "chưa có",
                LichSuBenh = entity.LichSuBenh ?? "không có"
            };
        }
    }
}
