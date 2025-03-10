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
                MaDonThuoc = dto.MaDonThuoc,
                MaThuoc = dto.MaThuoc,
                SoLuong = dto.SoLuong,
                CachDung = dto.CachDung
            };
        }

        public void MapDtoToEntity(DonThuocChiTietDTO dto, TblDonThuocChiTiet entity)
        {
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.MaThuoc = dto.MaThuoc;
            entity.SoLuong = dto.SoLuong;
            entity.CachDung = dto.CachDung;
        }

        public DonThuocChiTietDTO MapEntityToDto(TblDonThuocChiTiet entity)
        {
            return new DonThuocChiTietDTO
            {
                MaDonThuoc = entity.MaDonThuoc,
                MaThuoc = entity.MaThuoc,
                SoLuong = entity.SoLuong,
                CachDung = entity.CachDung
            };
        }
    }
}
