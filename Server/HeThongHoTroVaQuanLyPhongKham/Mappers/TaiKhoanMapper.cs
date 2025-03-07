using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class TaiKhoanMapper : IMapper<TaiKhoanDTO, TblTaiKhoan>
    {
        public TblTaiKhoan MapDtoToEntity(TaiKhoanDTO dto)
        {
            return new TblTaiKhoan
            {
                MaTaiKhoan = dto.MaTaiKhoan,
                TenDangNhap = dto.TenDangNhap,
                MatKhau = dto.MatKhau
            };
        }

        public void MapDtoToEntity(TaiKhoanDTO dto, TblTaiKhoan entity)
        {
            entity.MaTaiKhoan = dto.MaTaiKhoan;
            entity.TenDangNhap = dto.TenDangNhap;
            entity.MatKhau = dto.MatKhau;
        }

        public TaiKhoanDTO MapEntityToDto(TblTaiKhoan entity)
        {
            return new TaiKhoanDTO
            {
                MaTaiKhoan = entity.MaTaiKhoan,
                TenDangNhap = entity.TenDangNhap,
                MatKhau = entity.MatKhau
            };
        }
    }
}
