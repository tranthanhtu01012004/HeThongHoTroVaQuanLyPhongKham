using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<TblNhanVien> _nhanVienRepository;
        private readonly IRepository<TblBenhNhan> _benhNhanRepository;

        public UserService(
            IRepository<TblNhanVien> nhanVienRepository,
            IRepository<TblBenhNhan> benhNhanRepository)
        {
            _nhanVienRepository = nhanVienRepository;
            _benhNhanRepository = benhNhanRepository;
        }

        public async Task<int> GetMaBenhNhanFromTaiKhoan(int maTaiKhoan)
        {
            var entity = await _benhNhanRepository.GetQueryable()
                .FirstOrDefaultAsync(bn => bn.MaTaiKhoan == maTaiKhoan);
            if (entity is null)
                throw new NotFoundException("Không tìm thấy bệnh nhân");

            return entity.MaBenhNhan;
        }

        public async Task<int> GetMaNhanVienFromTaiKhoan(int maTaiKhoan)
        {
            var entity = await _nhanVienRepository.GetQueryable()
                .FirstOrDefaultAsync(nv => nv.MaTaiKhoan == maTaiKhoan);
            if (entity is null)
                throw new NotFoundException("Không tìm thấy nhân viên");

            return entity.MaNhanVien;
        }
    }
}