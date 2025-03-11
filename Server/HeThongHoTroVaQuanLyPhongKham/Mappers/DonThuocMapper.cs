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
                NgayKeDon = dto.NgayKeDon,
                TblDonThuocChiTiets = dto.ChiTietThuoc?
                                        .Select(ct => _donThuocChiTietMapping.MapDtoToEntity(ct))
                                            .ToList() ?? new List<TblDonThuocChiTiet>()
            };
        }

        public void MapDtoToEntity(DonThuocDTO dto, TblDonThuoc entity)
        {
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.NgayKeDon = dto.NgayKeDon;
            entity.TblDonThuocChiTiets = dto.ChiTietThuoc?
                .Select(ct => _donThuocChiTietMapping.MapDtoToEntity(ct))
                    .ToList() ?? new List<TblDonThuocChiTiet>();
        }

        public DonThuocDTO MapEntityToDto(TblDonThuoc entity)
        {
            return new DonThuocDTO
            {
                MaDonThuoc = entity.MaDonThuoc,
                NgayKeDon = entity.NgayKeDon,
                ChiTietThuoc = entity.TblDonThuocChiTiets?
                    .Select(ct => _donThuocChiTietMapping.MapEntityToDto(ct))
                        .ToList() ?? new List<DonThuocChiTietDTO>()
            };
        }
    }
}
