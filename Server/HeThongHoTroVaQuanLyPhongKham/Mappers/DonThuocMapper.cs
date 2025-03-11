using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class DonThuocMapper : IMapper<DonThuocDTO, TblDonThuoc>
    {
        private readonly IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> _donThuocChiTietMapping;

        public DonThuocMapper(IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> donThuocChiTietMapping)
        {
            _donThuocChiTietMapping = donThuocChiTietMapping;
        }

        public TblDonThuoc MapDtoToEntity(DonThuocDTO dto)
        {
            return new TblDonThuoc
            {
                MaDonThuoc = dto.MaDonThuoc,
                MaHoSoYte = dto.MaHoSoYte,
                NgayKeDon = dto.NgayKeDon,
                TblDonThuocChiTiets = dto.ChiTietThuocList?
                                        .Select(ct => _donThuocChiTietMapping.MapDtoToEntity(ct))
                                            .ToList() ?? new List<TblDonThuocChiTiet>()
            };
        }

        public void MapDtoToEntity(DonThuocDTO dto, TblDonThuoc entity)
        {
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.MaHoSoYte = dto.MaHoSoYte;
            entity.NgayKeDon = dto.NgayKeDon;
            entity.TblDonThuocChiTiets = dto.ChiTietThuocList?
                .Select(ct => _donThuocChiTietMapping.MapDtoToEntity(ct))
                    .ToList() ?? new List<TblDonThuocChiTiet>();
        }

        public DonThuocDTO MapEntityToDto(TblDonThuoc entity)
        {
            return new DonThuocDTO
            {
                MaDonThuoc = entity.MaDonThuoc,
                MaHoSoYte = entity.MaHoSoYte,
                NgayKeDon = entity.NgayKeDon,
                ChiTietThuocList = entity.TblDonThuocChiTiets?
                    .Select(ct => _donThuocChiTietMapping.MapEntityToDto(ct))
                        .ToList() ?? new List<DonThuocChiTietDTO>()
            };
        }
    }
}
