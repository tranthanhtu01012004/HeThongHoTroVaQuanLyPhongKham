using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class NhanVienMapper : IMapper<NhanVienDTO, TblNhanVien>
    {
        public TblNhanVien MapDtoToEntity(NhanVienDTO dto)
        {
            return new TblNhanVien
            {
                MaNhanVien = dto.MaNhanVien,
                MaTaiKhoan = dto.MaTaiKhoan,
                Ten = dto.Ten,
                SoDienThoai = dto.SoDienThoai,
                Email = dto.Email,
                CaLamViec = dto.CaLamViec,
                ChuyenMon = dto.ChuyenMon
            };
        }

        public void MapDtoToEntity(NhanVienDTO dto, TblNhanVien entity)
        {
            entity.MaNhanVien = dto.MaNhanVien;
            entity.MaTaiKhoan = dto.MaTaiKhoan;
            entity.Ten = dto.Ten;
            entity.SoDienThoai = dto.SoDienThoai;
            entity.Email = dto.Email;
            entity.CaLamViec = dto.CaLamViec;
            entity.ChuyenMon = dto.ChuyenMon;
        }

        public NhanVienDTO MapEntityToDto(TblNhanVien entity)
        {
            return new NhanVienDTO
            {
                MaNhanVien = entity.MaNhanVien,
                MaTaiKhoan = entity.MaTaiKhoan,
                Ten = entity.Ten,
                SoDienThoai = entity.SoDienThoai,
                Email = entity.Email,
                CaLamViec = entity.CaLamViec,
                ChuyenMon = entity.ChuyenMon
            };
        }
    }
}
