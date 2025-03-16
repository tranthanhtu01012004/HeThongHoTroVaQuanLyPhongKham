using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class PhongKhamNhanVienService : BaseService, IService<PhongKhamNhanVienDTO>
    {
        private readonly IRepository<TblPhongKhamNhanVien> _phongKhamNhanVienRepository;
        private readonly IMapper<PhongKhamNhanVienDTO, TblPhongKhamNhanVien> _phongKhamNhanVienMapping;
        private readonly IService<NhanVienDTO> _nhanVienService;
        private readonly IService<PhongKhamDTO> _phongKhamService;

        public PhongKhamNhanVienService(IRepository<TblPhongKhamNhanVien> phongKhamNhanVienRepository, IMapper<PhongKhamNhanVienDTO, TblPhongKhamNhanVien> phongKhamNhanVienMapping, IService<NhanVienDTO> nhanVienService, IService<PhongKhamDTO> phongKhamService)
        {
            _phongKhamNhanVienRepository = phongKhamNhanVienRepository;
            _phongKhamNhanVienMapping = phongKhamNhanVienMapping;
            _nhanVienService = nhanVienService;
            _phongKhamService = phongKhamService;
        }

        public async Task<PhongKhamNhanVienDTO> AddAsync(PhongKhamNhanVienDTO dto)
        {
            await _nhanVienService.GetByIdAsync(dto.MaNhanVien);
            await _phongKhamService.GetByIdAsync(dto.MaPhongKham);

            return _phongKhamNhanVienMapping.MapEntityToDto(
                await _phongKhamNhanVienRepository.CreateAsync(
                _phongKhamNhanVienMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _phongKhamNhanVienRepository.DeleteAsync(
                _phongKhamNhanVienMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<PhongKhamNhanVienDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _phongKhamNhanVienRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var phongKhamNhanViens = await _phongKhamNhanVienRepository.FindAllAsync(page, pageSize, pageSkip, "MaPhongKhamNhanVien");
            var dtoList = phongKhamNhanViens.Select(t => _phongKhamNhanVienMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<PhongKhamNhanVienDTO> GetByIdAsync(int id)
        {
            var phongKhamNhanVien = await _phongKhamNhanVienRepository.FindByIdAsync(id, "MaPhongKhamNhanVien");
            if (phongKhamNhanVien is null)
                throw new NotFoundException($"PhongKhamNhanVien với ID [{id}] không tồn tại.");

            return _phongKhamNhanVienMapping.MapEntityToDto(phongKhamNhanVien);
        }

        public async Task<PhongKhamNhanVienDTO> UpdateAsync(PhongKhamNhanVienDTO dto)
        {
            var phongKhamNhanVienUpdate = await _phongKhamNhanVienRepository.FindByIdForUpdateAsync(dto.MaPhongKhamNhanVien, "MaPhongKhamNhanVien");
            if (phongKhamNhanVienUpdate is null)
                throw new Exception($"Không tìm thấy phân công với MaPhongKhamNhanVien = {dto.MaPhongKhamNhanVien}");

            await _nhanVienService.GetByIdAsync(dto.MaNhanVien);
            await _phongKhamService.GetByIdAsync(dto.MaPhongKham);

            _phongKhamNhanVienMapping.MapDtoToEntity(dto, phongKhamNhanVienUpdate);

            return _phongKhamNhanVienMapping.MapEntityToDto(
                await _phongKhamNhanVienRepository.UpdateAsync(phongKhamNhanVienUpdate));
        }
    }
}
