using HeThongHoTroVaQuanLydonThuoc.Services;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri
{
    public class KetQuaDieuTriService : BaseService, IKetQuaDieuTriService
    {
        private readonly IRepository<TblKetQuaDieuTri> _ketQuaDieuTriRepository;
        private readonly IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> _ketQuaDieuTriMapping;
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;
        private readonly IRepository<TblDonThuoc> _donThuocRepository;

        public KetQuaDieuTriService(IRepository<TblKetQuaDieuTri> ketQuaDieuTriRepository, IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> ketQuaDieuTriMapping, IRepository<TblHoSoYTe> hoSoYTeRepository, IRepository<TblDonThuoc> donThuocRepository)
        {
            _ketQuaDieuTriRepository = ketQuaDieuTriRepository;
            _ketQuaDieuTriMapping = ketQuaDieuTriMapping;
            _hoSoYTeRepository = hoSoYTeRepository;
            _donThuocRepository = donThuocRepository;
        }

        public async Task<KetQuaDieuTriDTO> AddAsync(KetQuaDieuTriDTO dto)
        {
            // Su dung Repo cua HoSoYTe thay vi Service de tranh loi 'circular dependency'
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            // tuong tu
            var donThuoc = await _donThuocRepository.FindByIdAsync(dto.MaDonThuoc, "MaDonThuoc");
            if (hoSoYTe is null)
                throw new NotFoundException($"Mã đơn thuốc với ID [{dto.MaDonThuoc}] không tồn tại.");

            return _ketQuaDieuTriMapping.MapEntityToDto(
                await _ketQuaDieuTriRepository.CreateAsync(
                    _ketQuaDieuTriMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _ketQuaDieuTriRepository.DeleteAsync(
                _ketQuaDieuTriMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task DeleteByMaHoSoYTeAsync(int id)
        {
            var ketQuaDieuTris = await _ketQuaDieuTriRepository.GetQueryable()
                                        .Where(kq => kq.MaHoSoYte == id)
                                            .ToListAsync();
            if (ketQuaDieuTris.Any())
                await _ketQuaDieuTriRepository.DeleteAsync(ketQuaDieuTris);
        }

        public async Task<IEnumerable<KetQuaDieuTriDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var kqDieuTris = await _ketQuaDieuTriRepository.FindAllAsync(page, pageSize, pageSkip, "MaKetQuaDieuTri");
            return kqDieuTris.Select(t => _ketQuaDieuTriMapping.MapEntityToDto(t));
        }

        public async Task<KetQuaDieuTriDTO> GetByIdAsync(int id)
        {
            var kqDieuTri = await _ketQuaDieuTriRepository.FindByIdAsync(id, "MaKetQuaDieuTri");
            if (kqDieuTri is null)
                throw new NotFoundException($"Kết quả điều trị với ID [{id}] không tồn tại.");

            return _ketQuaDieuTriMapping.MapEntityToDto(kqDieuTri);
        }

        public async Task<KetQuaDieuTriDTO> UpdateAsync(KetQuaDieuTriDTO dto)
        {
            // Su dung Repo cua HoSoYTe thay vi Service de tranh loi 'circular dependency'
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            // tuong tu
            var donThuoc = await _donThuocRepository.FindByIdAsync(dto.MaDonThuoc, "MaDonThuoc");
            if (hoSoYTe is null)
                throw new NotFoundException($"Mã đơn thuốc với ID [{dto.MaDonThuoc}] không tồn tại.");

            var kqDieuTriUpdate = _ketQuaDieuTriMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaKetQuaDieuTri));

            _ketQuaDieuTriMapping.MapDtoToEntity(dto, kqDieuTriUpdate);

            return _ketQuaDieuTriMapping.MapEntityToDto(
                await _ketQuaDieuTriRepository.UpdateAsync(kqDieuTriUpdate));
        }
    }
}
