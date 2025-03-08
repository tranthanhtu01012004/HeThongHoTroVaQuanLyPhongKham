using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class PhongKhamMapper : IMapper<PhongKhamDTO, TblPhongKham>
    {
        public TblPhongKham MapDtoToEntity(PhongKhamDTO dto)
        {
            return new TblPhongKham
            {
               MaPhongKham = dto.MaPhongKham,
               Loai = dto.Loai,
               SucChua = dto.SucChua
            };
        }

        public void MapDtoToEntity(PhongKhamDTO dto, TblPhongKham entity)
        {
            entity.MaPhongKham = dto.MaPhongKham;
            entity.Loai = dto.Loai;
            entity.SucChua = dto.SucChua;
        }

        public PhongKhamDTO MapEntityToDto(TblPhongKham entity)
        {
            return new PhongKhamDTO
            {
                MaPhongKham = entity.MaPhongKham,
                Loai = entity.Loai,
                SucChua = entity.SucChua
            };
        }
    }
}
