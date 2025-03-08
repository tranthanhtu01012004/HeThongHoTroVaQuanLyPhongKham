using System.Text;
using HeThongHoTroVaQuanLyPhongKham.Data;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dependency Injection
// Mapping
builder.Services.AddScoped<IMapper<TaiKhoanDTO, TblTaiKhoan>, TaiKhoanMapper>();
builder.Services.AddScoped<IMapper<NhanVienDTO, TblNhanVien>, NhanVienMapper>();
builder.Services.AddScoped<IMapper<PhongKhamDTO,TblPhongKham>, PhongKhamMapper>();
builder.Services.AddScoped<IMapper<PhongKhamNhanVienDTO, TblPhongKhamNhanVien>, PhongKhamNhanVienMapper>();
builder.Services.AddScoped<IMapper<DichVuYTeDTO, TblDichVuYTe>, DichVuYTeMapper>();
builder.Services.AddScoped<IMapper<ThuocDTO, TblThuoc>, ThuocMapper>();
builder.Services.AddScoped<IMapper<LichHenDTO, TblLichHen>, LichHenMapper>();

// Repo
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();

// Service
builder.Services.AddScoped<IService<TaiKhoanDTO>, TaiKhoanService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IService<NhanVienDTO>, NhanVienService>();
builder.Services.AddScoped<IService<PhongKhamDTO>, PhongKhamService>();
builder.Services.AddScoped<IService<PhongKhamNhanVienDTO>, PhongKhamNhanVienService>();
builder.Services.AddScoped<IService<DichVuYTeDTO>, DichVuYTeService>();
builder.Services.AddScoped<IService<ThuocDTO>, ThuocService>();
builder.Services.AddScoped<ILichHenService, LichHenService>();

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
