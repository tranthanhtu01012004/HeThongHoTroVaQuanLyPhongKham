using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;

namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public class KetQuaXetNghiemMapper : IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem>
    {
        public TblKetQuaXetNghiem MapDtoToEntity(KetQuaXetNghiemDTO dto)
        {
            return new TblKetQuaXetNghiem
            {
                MaKetQua = dto.MaKetQua,
                MaHoSoYte = dto.MaHoSoYte,
                TenXetNghiem = dto.TenXetNghiem,
                KetQua = dto.KetQua,
                NgayXetNghiem = dto.NgayXetNghiem
            };
        }

        public void MapDtoToEntity(KetQuaXetNghiemDTO dto, TblKetQuaXetNghiem entity)
        {
            entity.MaKetQua = dto.MaKetQua;
            entity.MaHoSoYte = dto.MaHoSoYte;
            entity.TenXetNghiem = dto.TenXetNghiem;
            entity.KetQua = dto.KetQua;
            entity.NgayXetNghiem = dto.NgayXetNghiem;
        }

        public KetQuaXetNghiemDTO MapEntityToDto(TblKetQuaXetNghiem entity)
        {
            return new KetQuaXetNghiemDTO
            {
                MaKetQua = entity.MaKetQua,
                MaHoSoYte = entity.MaHoSoYte,
                TenXetNghiem = entity.TenXetNghiem,
                KetQua = entity.KetQua,
                NgayXetNghiem = entity.NgayXetNghiem
            };
        }
    }
}
