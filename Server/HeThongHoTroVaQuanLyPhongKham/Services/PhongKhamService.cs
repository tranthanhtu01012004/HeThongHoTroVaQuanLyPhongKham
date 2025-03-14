using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class PhongKhamService : BaseService, IService<PhongKhamDTO>
    {
        private readonly IRepository<TblPhongKham> _phongKhamRepository;
        private readonly IMapper<PhongKhamDTO, TblPhongKham> _phongKhamMapping;

        public PhongKhamService(IRepository<TblPhongKham> phongKhamRepository, IMapper<PhongKhamDTO, TblPhongKham> phongKhamMapping)
        {
            _phongKhamRepository = phongKhamRepository;
            _phongKhamMapping = phongKhamMapping;
        }

        public async Task<PhongKhamDTO> AddAsync(PhongKhamDTO dto)
        {
            return _phongKhamMapping.MapEntityToDto(
                await _phongKhamRepository.CreateAsync(
                    _phongKhamMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _phongKhamRepository.DeleteAsync(
                _phongKhamMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<PhongKhamDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _phongKhamRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var phongKhams = await _phongKhamRepository.FindAllAsync(page, pageSize, pageSkip, "MaPhongKham");
            var dtoList = phongKhams.Select(t => _phongKhamMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<PhongKhamDTO> GetByIdAsync(int id)
        {
            var phongKham = await _phongKhamRepository.FindByIdAsync(id, "MaPhongKham");
            if (phongKham is null)
                throw new NotFoundException($"Phòng khám với ID [{id}] không tồn tại.");

            return _phongKhamMapping.MapEntityToDto(phongKham);
        }

        public async Task<PhongKhamDTO> UpdateAsync(PhongKhamDTO dto)
        {
            var phongKhamUpdate = _phongKhamMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaPhongKham));

            _phongKhamMapping.MapDtoToEntity(dto, phongKhamUpdate);

            return _phongKhamMapping.MapEntityToDto(
                await _phongKhamRepository.UpdateAsync(phongKhamUpdate));
        }
    }
}
