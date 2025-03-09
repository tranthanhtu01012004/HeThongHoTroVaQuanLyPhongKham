using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class DonThuocMapper : IMapper<DonThuocDTO, TblDonThuoc>
    {
        public TblDonThuoc MapDtoToEntity(DonThuocDTO dto)
        {
            return new TblDonThuoc
            {
                MaDonThuoc = dto.MaDonThuoc,
                MaHoSoYte = dto.MaHoSoYTe,
                LieuLuong = dto.LieuLuong
            };
        }

        public void MapDtoToEntity(DonThuocDTO dto, TblDonThuoc entity)
        {
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.MaHoSoYte = dto.MaHoSoYTe;
            entity.LieuLuong = dto.LieuLuong;
        }

        public DonThuocDTO MapEntityToDto(TblDonThuoc entity)
        {
            return new DonThuocDTO
            {
                MaDonThuoc = entity.MaDonThuoc,
                MaHoSoYTe = entity.MaHoSoYte,
                LieuLuong = entity.LieuLuong
            };
        }
    }
}
