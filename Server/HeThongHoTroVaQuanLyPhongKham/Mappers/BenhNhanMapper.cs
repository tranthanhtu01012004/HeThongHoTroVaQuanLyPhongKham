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
                MaTaiKhoan = dto.MaTaiKhoan,
                Tuoi = dto.Tuoi,
                GioiTinh = dto.GioiTinh,
                DiaChi = dto.DiaChi,
                SoDienThoai = dto.SoDienThoai,
            };
        }

        public void MapDtoToEntity(BenhNhanDTO dto, TblBenhNhan entity)
        {
            entity.MaBenhNhan = dto.MaBenhNhan;
            entity.Tuoi = dto.Tuoi;
            entity.GioiTinh = dto.GioiTinh;
            entity.DiaChi = dto.DiaChi;
            entity.SoDienThoai = dto.SoDienThoai;
            entity.MaTaiKhoan = dto.MaTaiKhoan;
        }

        public BenhNhanDTO MapEntityToDto(TblBenhNhan entity)
        {
            return new BenhNhanDTO
            {
                MaBenhNhan = entity.MaBenhNhan,
                MaTaiKhoan = entity.MaTaiKhoan,
                Tuoi = entity.Tuoi,
                GioiTinh = entity.GioiTinh,
                DiaChi = entity.DiaChi,
                SoDienThoai = entity.SoDienThoai,
            };
        }
    }
}
