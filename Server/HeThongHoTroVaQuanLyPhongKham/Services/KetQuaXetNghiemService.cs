using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class KetQuaXetNghiemService : BaseService, IService<KetQuaXetNghiemDTO>
    {
        private readonly IRepository<TblKetQuaXetNghiem> _ketQuaXetNghiemRepository;
        private readonly IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> _ketQuaXetNghiemMapping;
        private readonly IHoSoYTeService _hoSoYTeService;

        public KetQuaXetNghiemService(IRepository<TblKetQuaXetNghiem> ketQuaXetNghiemRepository, IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> ketQuaXetNghiemMapping, IHoSoYTeService hoSoYTeService)
        {
            _ketQuaXetNghiemRepository = ketQuaXetNghiemRepository;
            _ketQuaXetNghiemMapping = ketQuaXetNghiemMapping;
            _hoSoYTeService = hoSoYTeService;
        }

        public async Task<KetQuaXetNghiemDTO> AddAsync(KetQuaXetNghiemDTO dto)
        {
            await _hoSoYTeService.GetByIdAsync(dto.MaHoSoYte);

            return _ketQuaXetNghiemMapping.MapEntityToDto(
                await _ketQuaXetNghiemRepository.CreateAsync(
                    _ketQuaXetNghiemMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _ketQuaXetNghiemRepository.DeleteAsync(
                _ketQuaXetNghiemMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<KetQuaXetNghiemDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var ketQuaXetNghiem = await _ketQuaXetNghiemRepository.FindAllAsync(page, pageSize, pageSkip, "MaKetQua");
            return ketQuaXetNghiem.Select(t => _ketQuaXetNghiemMapping.MapEntityToDto(t));
        }

        public async Task<KetQuaXetNghiemDTO> GetByIdAsync(int id)
        {
            var ketQuaXN = await _ketQuaXetNghiemRepository.FindByIdAsync(id, "MaKetQua");
            if (ketQuaXN is null)
                throw new NotFoundException($"Kết quả xét nghiệm với ID [{id}] không tồn tại.");

            return _ketQuaXetNghiemMapping.MapEntityToDto(ketQuaXN);
        }

        public async Task<KetQuaXetNghiemDTO> UpdateAsync(KetQuaXetNghiemDTO dto)
        {
            await _hoSoYTeService.GetByIdAsync(dto.MaHoSoYte);

            var ketQuaXetNghiemUpdate = _ketQuaXetNghiemMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaKetQua));

            _ketQuaXetNghiemMapping.MapDtoToEntity(dto, ketQuaXetNghiemUpdate);

            return _ketQuaXetNghiemMapping.MapEntityToDto(
                await _ketQuaXetNghiemRepository.UpdateAsync(ketQuaXetNghiemUpdate));
        }
    }
}
