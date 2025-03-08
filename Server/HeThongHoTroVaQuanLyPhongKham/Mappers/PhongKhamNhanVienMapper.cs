using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class PhongKhamNhanVienMapper : IMapper<PhongKhamNhanVienDTO, TblPhongKhamNhanVien>
    {
        public TblPhongKhamNhanVien MapDtoToEntity(PhongKhamNhanVienDTO dto)
        {
            return new TblPhongKhamNhanVien
            {
                MaPhongKham = dto.MaPhongKham,
                MaNhanVien = dto.MaNhanVien,
                VaiTro = dto.VaiTro
            };
        }

        public void MapDtoToEntity(PhongKhamNhanVienDTO dto, TblPhongKhamNhanVien entity)
        {
            throw new NotImplementedException();
        }

        public PhongKhamNhanVienDTO MapEntityToDto(TblPhongKhamNhanVien entity)
        {
            return new PhongKhamNhanVienDTO
            {
                MaPhongKham = entity.MaPhongKham,
                MaNhanVien = entity.MaNhanVien,
                VaiTro = entity.VaiTro
            };
        }
    }
}
