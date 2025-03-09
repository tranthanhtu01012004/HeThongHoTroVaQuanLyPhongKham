using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services.HashPassword;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class TaiKhoanService : BaseService, IService<TaiKhoanDTO>
    {
        private readonly IRepository<TblTaiKhoan> _taiKhoanRepository;
        private readonly IMapper<TaiKhoanDTO, TblTaiKhoan> _taiKhoanMapping;
        private readonly ITaiKhoanRepository _taiKhoanRepo;
        private readonly IPasswordHasher _passwordHasher;

        public TaiKhoanService(IRepository<TblTaiKhoan> taiKhoanRepository, IMapper<TaiKhoanDTO, TblTaiKhoan> taiKhoanMapping, ITaiKhoanRepository taiKhoanRepo, IPasswordHasher passwordHasher)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _taiKhoanMapping = taiKhoanMapping;
            _taiKhoanRepo = taiKhoanRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<TaiKhoanDTO> AddAsync(TaiKhoanDTO dto)
        {
            var taiKhoanExisting = await _taiKhoanRepo.FindByNameAsync(dto.TenDangNhap);
            if (taiKhoanExisting != null && 
                string.Equals(taiKhoanExisting.TenDangNhap, dto.TenDangNhap, StringComparison.OrdinalIgnoreCase))
                throw new DuplicateEntityException($"Tài khoản với tên đăng nhập [{dto.TenDangNhap}] đã tồn tại.");

            var taiKhoanEntity = _taiKhoanMapping.MapDtoToEntity(dto);

            taiKhoanEntity.MaVaiTro = dto.TenDangNhap.ToLower().Equals("admin") ? 1 : null;
            taiKhoanEntity.MatKhau = _passwordHasher.HashPassword(dto.MatKhau);

            return _taiKhoanMapping.MapEntityToDto(
                await _taiKhoanRepository.CreateAsync(taiKhoanEntity));
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
            taiKhoanUpdate.MatKhau = _passwordHasher.HashPassword(dto.MatKhau);

            return _taiKhoanMapping.MapEntityToDto(
                await _taiKhoanRepository.UpdateAsync(taiKhoanUpdate));
        }
    }
}
