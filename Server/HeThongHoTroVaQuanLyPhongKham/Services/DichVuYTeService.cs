using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class DichVuYTeService : BaseService, IService<DichVuYTeDTO>
    {
        private readonly IRepository<TblDichVuYTe> _dichVuYTeRepository;
        private readonly IMapper<DichVuYTeDTO, TblDichVuYTe> _dichVuYTeMapping;

        public DichVuYTeService(IRepository<TblDichVuYTe> dichVuYTeRepository, IMapper<DichVuYTeDTO, TblDichVuYTe> dichVuYTeMapping)
        {
            _dichVuYTeRepository = dichVuYTeRepository;
            _dichVuYTeMapping = dichVuYTeMapping;
        }

        public async Task<DichVuYTeDTO> AddAsync(DichVuYTeDTO dto)
        {
            return _dichVuYTeMapping.MapEntityToDto(
                await _dichVuYTeRepository.CreateAsync(
                    _dichVuYTeMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _dichVuYTeRepository.DeleteAsync(
                _dichVuYTeMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<DichVuYTeDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _dichVuYTeRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var dichVuYTes = await _dichVuYTeRepository.FindAllAsync(page, pageSize, pageSkip, "MaDichVuYte");
            var dtoList = dichVuYTes.Select(t => _dichVuYTeMapping.MapEntityToDto(t));

            return (dtoList, totalItems, totalPages);
        }

        public async Task<DichVuYTeDTO> GetByIdAsync(int id)
        {
            var dichVuYTe = await _dichVuYTeRepository.FindByIdAsync(id, "MaDichVuYte");
            if (dichVuYTe is null)
                throw new NotFoundException($"Dịch vụ y tế với ID [{id}] không tồn tại.");

            return _dichVuYTeMapping.MapEntityToDto(dichVuYTe);
        }

        public async Task<DichVuYTeDTO> UpdateAsync(DichVuYTeDTO dto)
        {
            var dichVuYTeUpdate = _dichVuYTeMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaDichVuYTe));

            _dichVuYTeMapping.MapDtoToEntity(dto, dichVuYTeUpdate);

            return _dichVuYTeMapping.MapEntityToDto(
                await _dichVuYTeRepository.UpdateAsync(dichVuYTeUpdate));
        }
    }
}
