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
                MaTaiKhoan = (int)dto.MaTaiKhoan,
                Ten = dto.Ten,
                SoDienThoai = dto.SoDienThoai,
                CaLamViec = dto.CaLamViec,
                ChuyenMon = dto.ChuyenMon
            };
        }

        public void MapDtoToEntity(NhanVienDTO dto, TblNhanVien entity)
        {
            entity.MaNhanVien = dto.MaNhanVien;
            entity.MaTaiKhoan = (int)dto.MaTaiKhoan;
            entity.Ten = dto.Ten;
            entity.SoDienThoai = dto.SoDienThoai;
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
                CaLamViec = entity.CaLamViec,
                ChuyenMon = entity.ChuyenMon
            };
        }
    }
}
