using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services.HashPassword;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Common;
using System.Security.Claims;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class TaiKhoanService : BaseService, ITaiKhoanService
    {
        private readonly IMapper<TaiKhoanDTO, TblTaiKhoan> _taiKhoanMapping;
        private readonly ITaiKhoanRepository _taiKhoanRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IService<VaiTroDTO> _vaiTroService;
        private readonly IService<BenhNhanDTO> _benhNhanService;

        public TaiKhoanService(IMapper<TaiKhoanDTO, TblTaiKhoan> taiKhoanMapping, ITaiKhoanRepository taiKhoanRepository, IPasswordHasher passwordHasher, IService<VaiTroDTO> vaiTroService, IService<BenhNhanDTO> benhNhanService)
        {
            _taiKhoanMapping = taiKhoanMapping;
            _taiKhoanRepository = taiKhoanRepository;
            _passwordHasher = passwordHasher;
            _vaiTroService = vaiTroService;
            _benhNhanService = benhNhanService;
        }

        public async Task<TaiKhoanDTO> AddAsync(TaiKhoanDTO dto)
        {
            var taiKhoanExisting = await _taiKhoanRepository.FindByNameAsync(dto.TenDangNhap);
            if (taiKhoanExisting != null && 
                string.Equals(taiKhoanExisting.TenDangNhap, dto.TenDangNhap, StringComparison.OrdinalIgnoreCase))
                throw new DuplicateEntityException($"Tài khoản với tên đăng nhập [{dto.TenDangNhap}] đã tồn tại.");

            var taiKhoanEntity = _taiKhoanMapping.MapDtoToEntity(dto);

            taiKhoanEntity.MaVaiTro = dto.TenDangNhap.ToLower().Equals("admin") ? (int)VaiTro.QuanLy : (int)VaiTro.BenhNhan;
            taiKhoanEntity.MatKhau = _passwordHasher.HashPassword(dto.MatKhau);

            var taiKhoan = await _taiKhoanRepository.CreateAsync(taiKhoanEntity);

            if (taiKhoan.MaVaiTro == (int)VaiTro.BenhNhan)
            {
                var benhNhan = new BenhNhanDTO
                {
                    MaTaiKhoan = taiKhoan.MaTaiKhoan
                };
                await _benhNhanService.AddAsync(benhNhan);
            }

            return _taiKhoanMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<TaiKhoanDTO> AddNhanVienAsync(TaiKhoanDTO dto)
        {
            var taiKhoanExisting = await _taiKhoanRepository.FindByNameAsync(dto.TenDangNhap);
            if (taiKhoanExisting != null &&
                string.Equals(taiKhoanExisting.TenDangNhap, dto.TenDangNhap, StringComparison.OrdinalIgnoreCase))
                throw new DuplicateEntityException($"Tài khoản với tên đăng nhập [{dto.TenDangNhap}] đã tồn tại.");

            var taiKhoanEntity = _taiKhoanMapping.MapDtoToEntity(dto);

            taiKhoanEntity.MatKhau = _passwordHasher.HashPassword(dto.MatKhau);

            return _taiKhoanMapping.MapEntityToDto(
                await _taiKhoanRepository.CreateAsync(taiKhoanEntity));
        }

        public async Task DeleteAsync(int id)
        {
            var taiKhoan = await _taiKhoanRepository.FindByIdAsync(id);
            if (taiKhoan.TenDangNhap.ToLower().Equals("admin"))
                throw new NotFoundException("Không thể xóa tài khoản quản trị viên.");

            await _taiKhoanRepository.DeleteAsync(taiKhoan);
        }

        public async Task<(IEnumerable<TaiKhoanDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _taiKhoanRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var taiKhoans = await _taiKhoanRepository.FindAllAsync(page, pageSize, pageSkip, "MaTaiKhoan");
            var dtoList = taiKhoans.Select(t => _taiKhoanMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<TaiKhoanDTO> GetByIdAsync(int id)
        {
            var taiKhoan = await _taiKhoanRepository.FindByIdAsync(id, "MaTaiKhoan");
            if (taiKhoan is null)
                throw new NotFoundException($"Tài khoản với ID [{id}] không tồn tại.");

            return _taiKhoanMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<TaiKhoanDTO> UpdateAsync(TaiKhoanUpdateDTO dto)
        {
            var taiKhoanUpdate = await GetByIdAsync(dto.MaTaiKhoan);

            if (dto.MaVaiTro != null)
            {
                await _vaiTroService.GetByIdAsync((int)dto.MaVaiTro);
                taiKhoanUpdate.MaVaiTro = dto.MaVaiTro;
            }

            if (dto.MatKhau != null)
                taiKhoanUpdate.MatKhau = _passwordHasher.HashPassword(dto.MatKhau);

            return _taiKhoanMapping.MapEntityToDto(
                await _taiKhoanRepository.UpdateAsync(
                    _taiKhoanMapping.MapDtoToEntity(taiKhoanUpdate)));
        }

        public Task<TaiKhoanDTO> UpdateAsync(TaiKhoanDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
