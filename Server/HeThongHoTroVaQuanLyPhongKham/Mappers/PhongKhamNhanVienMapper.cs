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
                MaPhongKhamNhanVien = dto.MaPhongKhamNhanVien,
                MaPhongKham = dto.MaPhongKham,
                MaNhanVien = dto.MaNhanVien,
                VaiTro = dto.VaiTro
            };
        }

        public void MapDtoToEntity(PhongKhamNhanVienDTO dto, TblPhongKhamNhanVien entity)
        {
            entity.MaPhongKhamNhanVien = dto.MaPhongKhamNhanVien;
            entity.MaPhongKham = dto.MaPhongKham;
            entity.MaNhanVien = dto.MaNhanVien;
            entity.VaiTro = dto.VaiTro;
        }

        public PhongKhamNhanVienDTO MapEntityToDto(TblPhongKhamNhanVien entity)
        {
            return new PhongKhamNhanVienDTO
            {
                MaPhongKhamNhanVien = entity.MaPhongKhamNhanVien,
                MaPhongKham = entity.MaPhongKham,
                MaNhanVien = entity.MaNhanVien,
                VaiTro = entity.VaiTro
            };
        }
    }
}
