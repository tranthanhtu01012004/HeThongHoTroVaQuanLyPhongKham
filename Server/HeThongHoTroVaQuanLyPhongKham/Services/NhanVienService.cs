using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Common;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class NhanVienService : BaseService, IService<NhanVienDTO>
    {
        private readonly IRepository<TblNhanVien> _nhanVienRepository;
        private readonly IMapper<NhanVienDTO, TblNhanVien> _nhanVienMapping;

        public NhanVienService(IRepository<TblNhanVien> nhanVienRepository, IMapper<NhanVienDTO, TblNhanVien> taiKhoanMapping)
        {
            _nhanVienRepository = nhanVienRepository;
            _nhanVienMapping = taiKhoanMapping;
        }

        public async Task<NhanVienDTO> AddAsync(NhanVienDTO dto)
        {
            return _nhanVienMapping.MapEntityToDto(
                await _nhanVienRepository.CreateAsync(
                    _nhanVienMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _nhanVienRepository.DeleteAsync(
                _nhanVienMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<NhanVienDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var taiKhoans = await _nhanVienRepository.FindAllAsync(page, pageSize, pageSkip, "MaNhanVien");
            return taiKhoans.Select(t => _nhanVienMapping.MapEntityToDto(t));
        }

        public async Task<NhanVienDTO> GetByIdAsync(int id)
        {
            var taiKhoan = await _nhanVienRepository.FindByIdAsync(id, "MaNhanVien");
            if (taiKhoan is null)
                throw new NotFoundException($"Nhân viên với ID [{id}] không tồn tại.");

            return _nhanVienMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<NhanVienDTO> UpdateAsync(NhanVienDTO dto)
        {
            var taiKhoanUpdate = _nhanVienMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaNhanVien));

            _nhanVienMapping.MapDtoToEntity(dto, taiKhoanUpdate);

            return _nhanVienMapping.MapEntityToDto(
                await _nhanVienRepository.UpdateAsync(taiKhoanUpdate));
        }
    }
}
