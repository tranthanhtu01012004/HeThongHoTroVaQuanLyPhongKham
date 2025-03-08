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
            var nhanVien = await _nhanVienService.GetByIdAsync(dto.MaNhanVien);
            var phongKham = await _phongKhamService.GetByIdAsync(dto.MaPhongKham);

            if (nhanVien is null || nhanVien is null)
                throw new NotFoundException("Phòng khám hoặc nhân viên không tồn tại.");

            return _phongKhamNhanVienMapping.MapEntityToDto(
                await _phongKhamNhanVienRepository.CreateAsync(
                _phongKhamNhanVienMapping.MapDtoToEntity(dto)));
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PhongKhamNhanVienDTO>> GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PhongKhamNhanVienDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PhongKhamNhanVienDTO> UpdateAsync(PhongKhamNhanVienDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
