using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class TrieuChungService : BaseService, IService<TrieuChungDTO>
    {
        private readonly IRepository<TblTrieuChung> _trieuChungRepository;
        private readonly IMapper<TrieuChungDTO, TblTrieuChung> _trieuChungMapping;
        private readonly IHoSoYTeService _hoSoYTeService;

        public TrieuChungService(IRepository<TblTrieuChung> trieuChungRepository, IMapper<TrieuChungDTO, TblTrieuChung> trieuChungMapping, IHoSoYTeService hoSoYTeService)
        {
            _trieuChungRepository = trieuChungRepository;
            _trieuChungMapping = trieuChungMapping;
            _hoSoYTeService = hoSoYTeService;
        }

        public async Task<TrieuChungDTO> AddAsync(TrieuChungDTO dto)
        {
            await _hoSoYTeService.GetByIdAsync(dto.MaHoSoYte);

            return _trieuChungMapping.MapEntityToDto(
                await _trieuChungRepository.CreateAsync(
                    _trieuChungMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _trieuChungRepository.DeleteAsync(
                _trieuChungMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<TrieuChungDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var trieuChungs = await _trieuChungRepository.FindAllAsync(page, pageSize, pageSkip, "MaTrieuChung");
            return trieuChungs.Select(t => _trieuChungMapping.MapEntityToDto(t));
        }

        public async Task<TrieuChungDTO> GetByIdAsync(int id)
        {
            var trieuChung = await _trieuChungRepository.FindByIdAsync(id, "MaTrieuChung");
            if (trieuChung is null)
                throw new NotFoundException($"Mã triệu chứng với ID [{id}] không tồn tại.");

            return _trieuChungMapping.MapEntityToDto(trieuChung);
        }

        public async Task<TrieuChungDTO> UpdateAsync(TrieuChungDTO dto)
        {
            var trieuChungUpdate = _trieuChungMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaTrieuChung));

            _trieuChungMapping.MapDtoToEntity(dto, trieuChungUpdate);

            return _trieuChungMapping.MapEntityToDto(
                await _trieuChungRepository.UpdateAsync(trieuChungUpdate));
        }
    }
}
