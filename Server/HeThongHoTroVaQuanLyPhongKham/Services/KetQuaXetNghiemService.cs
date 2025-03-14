using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class KetQuaXetNghiemService : BaseService, IKetQuaXetNghiem
    {
        private readonly IRepository<TblKetQuaXetNghiem> _ketQuaXetNghiemRepository;
        private readonly IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> _ketQuaXetNghiemMapping;
        //private readonly IHoSoYTeService _hoSoYTeService;
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository; // Tranh Circular Dependency

        public KetQuaXetNghiemService(IRepository<TblKetQuaXetNghiem> ketQuaXetNghiemRepository, IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> ketQuaXetNghiemMapping, IRepository<TblHoSoYTe> hoSoYTeRepository)
        {
            _ketQuaXetNghiemRepository = ketQuaXetNghiemRepository;
            _ketQuaXetNghiemMapping = ketQuaXetNghiemMapping;
            _hoSoYTeRepository = hoSoYTeRepository;
        }

        public async Task<KetQuaXetNghiemDTO> AddAsync(KetQuaXetNghiemDTO dto)
        {
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

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

        public async Task DeleteByMaHoSoYTeAsync(int id)
        {
            var kqXns = await _ketQuaXetNghiemRepository.GetQueryable()
                            .Where(kq => kq.MaHoSoYte == id)
                                .ToListAsync();
            if (kqXns.Any())
                await _ketQuaXetNghiemRepository.DeleteAsync(kqXns);
        }

        public async Task<(IEnumerable<KetQuaXetNghiemDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _ketQuaXetNghiemRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var ketQuaXetNghiem = await _ketQuaXetNghiemRepository.FindAllAsync(page, pageSize, pageSkip, "MaKetQua");
            var dtoList = ketQuaXetNghiem.Select(t => _ketQuaXetNghiemMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
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
            var ketQuaXetNghiemUpdate = _ketQuaXetNghiemMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaKetQua));

            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            _ketQuaXetNghiemMapping.MapDtoToEntity(dto, ketQuaXetNghiemUpdate);

            return _ketQuaXetNghiemMapping.MapEntityToDto(
                await _ketQuaXetNghiemRepository.UpdateAsync(ketQuaXetNghiemUpdate));
        }
    }
}
