using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class DichVuYTeMapper : IMapper<DichVuYTeDTO, TblDichVuYTe>
    {
        public TblDichVuYTe MapDtoToEntity(DichVuYTeDTO dto)
        {
            return new TblDichVuYTe
            {
                MaDichVuYte = dto.MaDichVuYTe,
                Ten = dto.Ten,
                ChiPhi = dto.ChiPhi
            };
        }

        public void MapDtoToEntity(DichVuYTeDTO dto, TblDichVuYTe entity)
        {
            entity.MaDichVuYte = dto.MaDichVuYTe;
            entity.Ten = dto.Ten;
            entity.ChiPhi = dto.ChiPhi;
        }

        public DichVuYTeDTO MapEntityToDto(TblDichVuYTe entity)
        {
            return new DichVuYTeDTO
            {
                MaDichVuYTe = entity.MaDichVuYte,
                Ten = entity.Ten,
                ChiPhi = entity.ChiPhi
            };
        }
    }
}
