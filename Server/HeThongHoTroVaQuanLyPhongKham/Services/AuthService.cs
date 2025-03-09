using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services.HashPassword;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITaiKhoanRepository _taiKhoanRepository;

        private readonly IMapper<TaiKhoanDTO, TblTaiKhoan> _taiKhoanMapping;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(ITaiKhoanRepository taiKhoanRepository, IMapper<TaiKhoanDTO, TblTaiKhoan> taiKhoanMapping, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _taiKhoanMapping = taiKhoanMapping;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TaiKhoanDTO> GetUserWithRole(TaiKhoanDTO taiKhoanDTO)
        {
            var taiKhoan = await _taiKhoanRepository.FindByIdAsync(taiKhoanDTO.MaTaiKhoan);
            if (taiKhoan is null)
                throw new NotFoundException($"Không tìm thấy tài khoản với ID [{taiKhoanDTO.MaTaiKhoan}]");

            return _taiKhoanMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<LoginResponse> Login(TaiKhoanDTO taiKhoanDTO)
        {
            var taiKhoan = await _taiKhoanRepository.FindByNameAsync(taiKhoanDTO.TenDangNhap);
            if (taiKhoan == null)
                throw new NotFoundException($"Tài khoản với tên đăng nhập {taiKhoanDTO.TenDangNhap} không tồn tại");

            if (!_passwordHasher.VerifyPassword(taiKhoanDTO.MatKhau, taiKhoan.MatKhau))
                throw new UnauthorizedException("Mật khẩu không đúng");

            var token = _jwtService.GenerateToken(taiKhoan);
            var role = taiKhoan.MaVaiTroNavigation?.Ten ?? string.Empty;

            return new LoginResponse
            {
                Token = token,
                Role = role
            };
        }
    }
}
