using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class ThuocMapper : IMapper<ThuocDTO, TblThuoc>
    {
        public TblThuoc MapDtoToEntity(ThuocDTO dto)
        {
            return new TblThuoc
            {
                MaThuoc = dto.MaThuoc,
                Ten = dto.Ten,
                MoTa = dto.MoTa,
                DonVi = dto.DonVi
            };
        }

        public void MapDtoToEntity(ThuocDTO dto, TblThuoc entity)
        {
            entity.MaThuoc = dto.MaThuoc;
            entity.Ten = dto.Ten;
            entity.MoTa = dto.MoTa;
            entity.DonVi = dto.DonVi;
        }

        public ThuocDTO MapEntityToDto(TblThuoc entity)
        {
            return new ThuocDTO
            {
                MaThuoc = entity.MaThuoc,
                Ten = entity.Ten,
                MoTa = entity.MoTa,
                DonVi = entity.DonVi
            };
        }
    }
}
