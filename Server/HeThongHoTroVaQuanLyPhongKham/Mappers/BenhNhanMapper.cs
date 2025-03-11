using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class BenhNhanMapper : IMapper<BenhNhanDTO, TblBenhNhan>
    {
        public TblBenhNhan MapDtoToEntity(BenhNhanDTO dto)
        {
            return new TblBenhNhan
            {
                MaBenhNhan = dto.MaBenhNhan,
                Tuoi = dto.Tuoi,
                GioiTinh = dto.GioiTinh,
                DiaChi = dto.DiaChi,
                SoDienThoai = dto.SoDienThoai,
                Email = dto.Email
            };
        }

        public void MapDtoToEntity(BenhNhanDTO dto, TblBenhNhan entity)
        {
            entity.MaBenhNhan = dto.MaBenhNhan;
            entity.Tuoi = dto.Tuoi;
            entity.GioiTinh = dto.GioiTinh;
            entity.DiaChi = dto.DiaChi;
            entity.SoDienThoai = dto.SoDienThoai;
            entity.Email = dto.Email;
        }

        public BenhNhanDTO MapEntityToDto(TblBenhNhan entity)
        {
            return new BenhNhanDTO
            {
                MaBenhNhan = entity.MaBenhNhan,
                Tuoi = entity.Tuoi,
                GioiTinh = entity.GioiTinh,
                DiaChi = entity.DiaChi,
                SoDienThoai = entity.SoDienThoai,
                Email = entity.Email
            };
        }
    }
}
