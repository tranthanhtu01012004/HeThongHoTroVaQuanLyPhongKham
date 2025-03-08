using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class TaiKhoanService : BaseService, IService<TaiKhoanDTO>
    {
        private readonly IRepository<TblTaiKhoan> _taiKhoanRepository;
        private readonly IMapper<TaiKhoanDTO, TblTaiKhoan> _taiKhoanMapping;
        private readonly ITaiKhoanRepository _taiKhoanRepo;

        public TaiKhoanService(IRepository<TblTaiKhoan> taiKhoanRepository, IMapper<TaiKhoanDTO, TblTaiKhoan> taiKhoanMapping, ITaiKhoanRepository taiKhoanRepo)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _taiKhoanMapping = taiKhoanMapping;
            _taiKhoanRepo = taiKhoanRepo;
        }

        public async Task<TaiKhoanDTO> AddAsync(TaiKhoanDTO dto)
        {
            var taiKhoanExisting = await _taiKhoanRepo.FindByNameAsync(dto.TenDangNhap);
            if (taiKhoanExisting != null && 
                string.Equals(taiKhoanExisting.TenDangNhap, dto.TenDangNhap, StringComparison.OrdinalIgnoreCase))
                throw new DuplicateEntityException($"Tài khoản với tên đăng nhập [{dto.TenDangNhap}] đã tồn tại.");

            var taiKhoan = await _taiKhoanRepository.CreateAsync(_taiKhoanMapping.MapDtoToEntity(dto));
            return _taiKhoanMapping.MapEntityToDto(taiKhoan);
        }

        public async Task DeleteAsync(int id)
        {
            await _taiKhoanRepository.DeleteAsync(
                _taiKhoanMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<TaiKhoanDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var taiKhoans = await _taiKhoanRepository.FindAllAsync(page, pageSize, pageSkip, "MaTaiKhoan");
            return taiKhoans.Select(t => _taiKhoanMapping.MapEntityToDto(t));
        }

        public async Task<TaiKhoanDTO> GetByIdAsync(int id)
        {
            var taiKhoan = await _taiKhoanRepository.FindByIdAsync(id, "MaTaiKhoan");
            if (taiKhoan is null)
                throw new NotFoundException($"Tài khoản với ID [{id}] không tồn tại.");

            return _taiKhoanMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<TaiKhoanDTO> UpdateAsync(TaiKhoanDTO dto)
        {
            var taiKhoanUpdate = _taiKhoanMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaTaiKhoan));

            _taiKhoanMapping.MapDtoToEntity(dto, taiKhoanUpdate);

            return _taiKhoanMapping.MapEntityToDto(
                await _taiKhoanRepository.UpdateAsync(taiKhoanUpdate));
        }
    }
}
