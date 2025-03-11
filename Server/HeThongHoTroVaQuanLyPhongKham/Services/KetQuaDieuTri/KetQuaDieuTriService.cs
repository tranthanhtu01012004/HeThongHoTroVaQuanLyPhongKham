using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri
{
    public class KetQuaDieuTriService : BaseService, IService<KetQuaDieuTriDTO>
    {
        private readonly IRepository<TblKetQuaDieuTri> _ketQuaDieuTriRepository;
        private readonly IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> _ketQuaDieuTriMapping;
        private readonly IService<HoSoYTeDTO> _hoSoYTeService;
        private readonly IService<DonThuocDTO> _donThuocService;

        public KetQuaDieuTriService(IRepository<TblKetQuaDieuTri> ketQuaDieuTriRepository, IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> ketQuaDieuTriMapping, IService<HoSoYTeDTO> hoSoYTeService, IService<DonThuocDTO> donThuocService)
        {
            _ketQuaDieuTriRepository = ketQuaDieuTriRepository;
            _ketQuaDieuTriMapping = ketQuaDieuTriMapping;
            _hoSoYTeService = hoSoYTeService;
            _donThuocService = donThuocService;
        }

        public async Task<KetQuaDieuTriDTO> AddAsync(KetQuaDieuTriDTO dto)
        {
            await _hoSoYTeService.GetByIdAsync(dto.MaHoSoYte);
            await _donThuocService.GetByIdAsync(dto.MaDonThuoc);

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
            await _hoSoYTeService.GetByIdAsync(dto.MaHoSoYte);
            await _donThuocService.GetByIdAsync(dto.MaDonThuoc);

            var kqDieuTriUpdate = _ketQuaDieuTriMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaKetQuaDieuTri));

            _ketQuaDieuTriMapping.MapDtoToEntity(dto, kqDieuTriUpdate);

            return _ketQuaDieuTriMapping.MapEntityToDto(
                await _ketQuaDieuTriRepository.UpdateAsync(kqDieuTriUpdate));
        }
    }
}
