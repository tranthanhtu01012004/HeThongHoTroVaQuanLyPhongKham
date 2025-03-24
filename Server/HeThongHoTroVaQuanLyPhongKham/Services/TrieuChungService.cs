using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class TrieuChungService : BaseService, ITrieuChungService
    {
        private readonly IRepository<TblTrieuChung> _trieuChungRepository;
        private readonly IMapper<TrieuChungDTO, TblTrieuChung> _trieuChungMapping;
        //private readonly IService<HoSoYTeDTO> _hoSoYTeService;
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository; // Tranh Circular Dependency

        public TrieuChungService(IRepository<TblTrieuChung> trieuChungRepository, IMapper<TrieuChungDTO, TblTrieuChung> trieuChungMapping, IRepository<TblHoSoYTe> hoSoYTeRepository)
        {
            _trieuChungRepository = trieuChungRepository;
            _trieuChungMapping = trieuChungMapping;
            _hoSoYTeRepository = hoSoYTeRepository;
        }

        public async Task<TrieuChungDTO> AddAsync(TrieuChungDTO dto)
        {
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

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

        public async Task DeleteByMaHoSoYTeAsync(int id)
        {
            var trieuChungs = await _trieuChungRepository.GetQueryable()
                            .Where(kq => kq.MaHoSoYte == id)
                                .ToListAsync();
            if (trieuChungs.Any())
                await _trieuChungRepository.DeleteAsync(trieuChungs);
        }

        public async Task<(IEnumerable<TrieuChungDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _trieuChungRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var trieuChungs = await _trieuChungRepository.FindAllAsync(page, pageSize, pageSkip, "MaTrieuChung");
            var dtoList = trieuChungs.Select(t => _trieuChungMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
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

            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            _trieuChungMapping.MapDtoToEntity(dto, trieuChungUpdate);

            return _trieuChungMapping.MapEntityToDto(
                await _trieuChungRepository.UpdateAsync(trieuChungUpdate));
        }
    }
}
