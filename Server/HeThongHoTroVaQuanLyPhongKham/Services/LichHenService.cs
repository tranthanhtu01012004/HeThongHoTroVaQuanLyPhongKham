using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class LichHenService : BaseService, ILichHenService
    {
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IMapper<LichHenDTO, TblLichHen> _lichHenMapping;
        //private readonly IService<BenhNhanDTO> _benhNhanService;
        private readonly IRepository<TblBenhNhan> _benhNhanRepository; // Circular denpendency
        private readonly IService<NhanVienDTO> _nhanVienService;
        private readonly IService<DichVuYTeDTO> _dichVuYTeService;
        private readonly IService<PhongKhamDTO> _phongKhamService;
        private readonly IJwtService _jwtService;
        private readonly IRepository<TblHoaDon> _hoaDonRepository;

        public LichHenService(IRepository<TblLichHen> lichHenRepository, IMapper<LichHenDTO, TblLichHen> lichHenMapping, IRepository<TblBenhNhan> benhNhanRepository, IService<NhanVienDTO> nhanVienService, IService<DichVuYTeDTO> dichVuYTeService, IService<PhongKhamDTO> phongKhamService, IJwtService jwtService, IRepository<TblHoaDon> hoaDonRepository)
        {
            _lichHenRepository = lichHenRepository;
            _lichHenMapping = lichHenMapping;
            _benhNhanRepository = benhNhanRepository;
            _nhanVienService = nhanVienService;
            _dichVuYTeService = dichVuYTeService;
            _phongKhamService = phongKhamService;
            _jwtService = jwtService;
            _hoaDonRepository = hoaDonRepository;
        }

        public Task<LichHenDTO> AddAsync(LichHenDTO dto)
        {
            throw new NotImplementedException();
        }

        //public async Task<LichHenDTO> AddAsync(LichHenDTO dto)
        //{
        //    var benhNhan = await _benhNhanRepository.FindByIdAsync(dto.MaBenhNhan, "MaBenhNhan");
        //    if (benhNhan is null)
        //        throw new NotFoundException($"Bệnh nhân với ID [{dto.MaBenhNhan}] không tồn tại.");

        //    await _nhanVienService.GetByIdAsync(dto.MaNhanVien);
        //    await _dichVuYTeService.GetByIdAsync(dto.MaDichVuYTe);
        //    await _phongKhamService.GetByIdAsync(dto.MaPhongKham);

        //    return _lichHenMapping.MapEntityToDto(
        //        await _lichHenRepository.CreateAsync(
        //            _lichHenMapping.MapDtoToEntity(dto)));
        //}

        public async Task<LichHenDTO> AddForPatientAsync(LichHenCreateDTO dto)
        {
            await _dichVuYTeService.GetByIdAsync(dto.MaDichVuYTe);

            var maTaiKhoan = _jwtService.GetMaTaiKhoan();
            if (maTaiKhoan == null)
                throw new UnauthorizedAccessException("Không thể xác định mã tài khoản từ token.");

            var benhNhan = await _benhNhanRepository.GetQueryable()
                .FirstOrDefaultAsync(bn => bn.MaTaiKhoan == maTaiKhoan.Value);
            if (benhNhan is null)
                throw new NotFoundException($"Bệnh nhân với mã tài khoản [{maTaiKhoan}] không tồn tại (chưa đăng ký tài khoản).");

            var lichHen = new TblLichHen
            {
                MaDichVuYte = dto.MaDichVuYTe,
                NgayHen = dto.NgayHen,
                MaBenhNhan = benhNhan.MaBenhNhan,
                MaNhanVien = null,
                MaPhongKham = null,
                TrangThai = "Chờ xác nhận"
            };

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.CreateAsync(lichHen));
        }

        public async Task DeleteAsync(int id)
        {
            var lichHen = await GetByIdAsync(id);
            if (lichHen == null)
            {
                throw new Exception("Lịch hẹn không tồn tại.");
            }

            // Xóa tất cả hóa đơn liên quan trước
            var hoaDons = await _hoaDonRepository.GetQueryable()
                            .Where(hd => hd.MaLichHen == id)
                            .ToListAsync();
            await _hoaDonRepository.DeleteAsync(hoaDons);

            await _lichHenRepository.DeleteAsync(
                _lichHenMapping.MapDtoToEntity(lichHen));
        }

        public async Task<(IEnumerable<LichHenDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(
            int page, int pageSize, 
            DateTime? ngayHen = null, 
            int? maNhanVien = null, 
            int? maPhong = null)
        {
            var query = _lichHenRepository.GetQueryable();
            var totalItems = await _lichHenRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            if (ngayHen.HasValue)
                query = query.Where(lh => lh.NgayHen.Date == ngayHen.Value.Date);

            if (maNhanVien.HasValue)
                query = query.Where(lh => lh.MaNhanVien == maNhanVien.Value);

            if (maPhong.HasValue)
                query = query.Where(lh => lh.MaPhongKham == maPhong.Value);

            var lichHens = await _lichHenRepository.FindAllWithQueryAsync(query, page, pageSize, pageSkip, "MaLichHen");

            var dtoList = lichHens.Select(lh => _lichHenMapping.MapEntityToDto(lh));
            return (dtoList, totalItems, totalPages);
        }

        public Task<IEnumerable<LichHenDTO>> GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<LichHenDTO> GetByIdAsync(int id)
        {
            var lichHen = await _lichHenRepository.FindByIdAsync(id, "MaLichHen");
            if (lichHen is null)
                throw new NotFoundException($"Lịch hẹn với ID [{id}] không tồn tại.");

            return _lichHenMapping.MapEntityToDto(lichHen);
        }

        public async Task<LichHenDTO> UpdateAsync(LichHenForUpdateDTO dto)
        {
            var lichHen = await GetByIdAsync(dto.MaLichHen);
            var lichHenUpdate = _lichHenMapping.MapDtoToEntity(lichHen);

            await _nhanVienService.GetByIdAsync(dto.MaNhanVien);  
            lichHenUpdate.MaNhanVien = dto.MaNhanVien;

            await _phongKhamService.GetByIdAsync(dto.MaPhongKham);
            lichHenUpdate.MaPhongKham = dto.MaPhongKham;

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.UpdateAsync(lichHenUpdate));
        }

        public Task<LichHenDTO> UpdateAsync(LichHenDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<LichHenDTO> UpdateTrangThaiAsync(LichHenUpdateDTO dto)
        {
            var lichHen = await GetByIdAsync(dto.MaLichHen);

            if (lichHen.TrangThai == "Hủy")
                throw new InvalidOperationException("Lịch hẹn đã bị hủy, không thể thay đổi trạng thái.");

            if (lichHen.TrangThai.Equals("Đã xác nhận"))
                if (dto.TrangThai != "Đã hoàn thành")
                    throw new InvalidOperationException("Lịch hẹn đã xác nhận, không thể thay đổi trạng thái ngoại trừ trạng thái đã hoàn thành.");

            if (lichHen.TrangThai.Equals("Đã hoàn thành"))
                throw new UnauthorizedAccessException("Lịch hẹn đã hoàn thành, không thể thay đổi trạng thái.");

            var role = _jwtService.GetCurrentRole();
            if (dto.TrangThai.Equals("Đã hoàn thành") && role != "BacSi")
                throw new UnauthorizedAccessException("Chỉ bác sĩ mới được đánh dấu lịch hẹn là hoàn thành.");

            lichHen.TrangThai = dto.TrangThai;

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.UpdateAsync(_lichHenMapping.MapDtoToEntity(lichHen)));
        }

        Task<(IEnumerable<LichHenDTO> Items, int TotalItems, int TotalPages)> IService<LichHenDTO>.GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
