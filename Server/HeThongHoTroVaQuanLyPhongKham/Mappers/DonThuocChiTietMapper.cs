using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class DonThuocChiTietMapper : IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet>
    {
        public TblDonThuocChiTiet MapDtoToEntity(DonThuocChiTietDTO dto)
        {
            return new TblDonThuocChiTiet
            {
                MaDonThuocChiTiet = dto.MaDonThuocChiTiet,
                MaDonThuoc = dto.MaDonThuoc,
                MaThuoc = dto.MaThuoc,
                SoLuong = dto.SoLuong,
                CachDung = dto.CachDung,
                LieuLuong = dto.LieuLuong,
                TanSuat = dto.TanSuat
            };
        }

        public void MapDtoToEntity(DonThuocChiTietDTO dto, TblDonThuocChiTiet entity)
        {
            entity.MaDonThuocChiTiet = dto.MaDonThuocChiTiet;
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.MaThuoc = dto.MaThuoc;
            entity.SoLuong = dto.SoLuong;
            entity.CachDung = dto.CachDung;
            entity.LieuLuong = dto.LieuLuong;
            entity.TanSuat = dto.TanSuat;
        }

        public DonThuocChiTietDTO MapEntityToDto(TblDonThuocChiTiet entity)
        {
            return new DonThuocChiTietDTO
            {
                MaDonThuocChiTiet = entity.MaDonThuocChiTiet,
                MaDonThuoc = entity.MaDonThuoc,
                MaThuoc = entity.MaThuoc,
                SoLuong = entity.SoLuong,
                CachDung = entity.CachDung,
                LieuLuong = entity.LieuLuong,
                TanSuat = entity.TanSuat
            };
        }
    }
}
