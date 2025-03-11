using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class TrieuChungMapper : IMapper<TrieuChungDTO, TblTrieuChung>
    {
        public TblTrieuChung MapDtoToEntity(TrieuChungDTO dto)
        {
            return new TblTrieuChung
            {
                MaTrieuChung = dto.MaTrieuChung,
                MaHoSoYte = dto.MaHoSoYte,
                TenTrieuChung = dto.TenTrieuChung,
                MoTa = dto.MoTa,
                ThoiGianXuatHien = dto.ThoiGianXuatHien
            };
        }

        public void MapDtoToEntity(TrieuChungDTO dto, TblTrieuChung entity)
        {
            entity.MaTrieuChung = dto.MaTrieuChung;
            entity.MaHoSoYte = dto.MaHoSoYte;
            entity.TenTrieuChung = dto.TenTrieuChung;
            entity.MoTa = dto.MoTa;
            entity.ThoiGianXuatHien = dto.ThoiGianXuatHien;
        }

        public TrieuChungDTO MapEntityToDto(TblTrieuChung entity)
        {
            return new TrieuChungDTO
            {
                MaTrieuChung = entity.MaTrieuChung,
                MaHoSoYte = entity.MaHoSoYte,
                TenTrieuChung = entity.TenTrieuChung,
                MoTa = entity.MoTa,
                ThoiGianXuatHien = entity.ThoiGianXuatHien
            };
        }
    }
}
