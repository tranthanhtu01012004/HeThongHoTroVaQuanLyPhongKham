using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class KetQuaDieuTriMapper : IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri>
    {
        public TblKetQuaDieuTri MapDtoToEntity(KetQuaDieuTriDTO dto)
        {
            return new TblKetQuaDieuTri
            {
                MaKetQuaDieuTri = dto.MaKetQuaDieuTri,
                MaHoSoYte = dto.MaHoSoYte,
                MaDonThuoc = dto.MaDonThuoc,
                HieuQua = dto.HieuQua,
                TacDungPhu = dto.TacDungPhu,
                NgayDanhGia = dto.NgayDanhGia
            };
        }

        public void MapDtoToEntity(KetQuaDieuTriDTO dto, TblKetQuaDieuTri entity)
        {
            entity.MaKetQuaDieuTri = dto.MaKetQuaDieuTri;
            entity.MaHoSoYte = dto.MaHoSoYte;
            entity.MaDonThuoc = dto.MaDonThuoc;
            entity.HieuQua = dto.HieuQua;
            entity.TacDungPhu = dto.TacDungPhu;
            entity.NgayDanhGia = dto.NgayDanhGia;
        }

        public KetQuaDieuTriDTO MapEntityToDto(TblKetQuaDieuTri entity)
        {
            return new KetQuaDieuTriDTO
            {
                MaKetQuaDieuTri = entity.MaKetQuaDieuTri,
                MaHoSoYte = entity.MaHoSoYte,
                MaDonThuoc = entity.MaDonThuoc,
                HieuQua = entity.HieuQua,
                TacDungPhu = entity.TacDungPhu,
                NgayDanhGia = entity.NgayDanhGia
            };
        }
    }
}
