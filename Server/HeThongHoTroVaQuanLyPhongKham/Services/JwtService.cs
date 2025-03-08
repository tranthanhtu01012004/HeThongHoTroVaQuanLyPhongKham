using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Models;
using Microsoft.IdentityModel.Tokens;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(TblTaiKhoan taiKhoan)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTaiKhoan.ToString()),
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap)
            };

            if (taiKhoan.MaVaiTroNavigation != null)
                claims.Add(new Claim(ClaimTypes.Role, taiKhoan.MaVaiTroNavigation.Ten));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
