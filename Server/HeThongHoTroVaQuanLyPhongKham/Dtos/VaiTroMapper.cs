using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class VaiTroMapper : IMapper<VaiTroDTO, TblVaiTro>
    {
        public TblVaiTro MapDtoToEntity(VaiTroDTO dto)
        {
            return new TblVaiTro
            {
                MaVaiTro = dto.MaVaiTro,
                Ten = dto.Ten
            };
        }

        public void MapDtoToEntity(VaiTroDTO dto, TblVaiTro entity)
        {
            entity.MaVaiTro = dto.MaVaiTro;
            entity.Ten = dto.Ten;
        }

        public VaiTroDTO MapEntityToDto(TblVaiTro entity)
        {
            return new VaiTroDTO
            {
                MaVaiTro = entity.MaVaiTro,
                Ten = entity.Ten
            };
        }
    }
}
