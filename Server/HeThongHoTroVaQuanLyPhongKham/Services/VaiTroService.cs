using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class VaiTroService : BaseService, IVaiTroService
    {
        private readonly IRepository<TblVaiTro> _vaiTroRepository;
        private readonly IMapper<VaiTroDTO, TblVaiTro> _vaiTroMapping;

        public VaiTroService(IRepository<TblVaiTro> vaiTroRepository, IMapper<VaiTroDTO, TblVaiTro> vaiTroMapping)
        {
            _vaiTroRepository = vaiTroRepository;
            _vaiTroMapping = vaiTroMapping;
        }

        public async Task<VaiTroDTO> AddAsync(VaiTroDTO dto)
        {
            return _vaiTroMapping.MapEntityToDto(
                await _vaiTroRepository.CreateAsync(
                    _vaiTroMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _vaiTroRepository.DeleteAsync(
                _vaiTroMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<VaiTroDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _vaiTroRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var vaiTros = await _vaiTroRepository.FindAllAsync(page, pageSize, pageSkip, "MaVaiTro");
            var dtoList = vaiTros.Select(t => _vaiTroMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<IEnumerable<VaiTroDTO>> GetAllAsync()
        {
            var vaiTros = await _vaiTroRepository.FindAllAsync("MaVaiTro");
            return vaiTros.Select(t => _vaiTroMapping.MapEntityToDto(t));
        }

        public async Task<VaiTroDTO> GetByIdAsync(int id)
        {
            var vaiTro = await _vaiTroRepository.FindByIdAsync(id, "MaVaiTro");
            if (vaiTro is null)
                throw new NotFoundException($"Vai trò với ID [{id}] không tồn tại.");

            return _vaiTroMapping.MapEntityToDto(vaiTro);
        }

        public async Task<VaiTroDTO> UpdateAsync(VaiTroDTO dto)
        {
            var vaiTroUpdate = _vaiTroMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaVaiTro));

            _vaiTroMapping.MapDtoToEntity(dto, vaiTroUpdate);

            return _vaiTroMapping.MapEntityToDto(
                await _vaiTroRepository.UpdateAsync(vaiTroUpdate));
        }
    }
}
