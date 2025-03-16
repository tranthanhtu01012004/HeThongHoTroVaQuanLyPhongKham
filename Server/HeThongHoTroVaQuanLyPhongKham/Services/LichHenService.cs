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

        public LichHenService(IRepository<TblLichHen> lichHenRepository, IMapper<LichHenDTO, TblLichHen> lichHenMapping, IRepository<TblBenhNhan> benhNhanRepository, IService<NhanVienDTO> nhanVienService, IService<DichVuYTeDTO> dichVuYTeService, IService<PhongKhamDTO> phongKhamService, IJwtService jwtService)
        {
            _lichHenRepository = lichHenRepository;
            _lichHenMapping = lichHenMapping;
            _benhNhanRepository = benhNhanRepository;
            _nhanVienService = nhanVienService;
            _dichVuYTeService = dichVuYTeService;
            _phongKhamService = phongKhamService;
            _jwtService = jwtService;
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
            await _lichHenRepository.DeleteAsync(
                _lichHenMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
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

        public async Task<LichHenDTO> UpdateAsync(LichHenDTO dto)
        {
            var lichHen = await GetByIdAsync(dto.MaLichHen);
            var lichHenUpdate = _lichHenMapping.MapDtoToEntity(lichHen);

            if (lichHen.MaBenhNhan != dto.MaBenhNhan)
                throw new InvalidOperationException("Không được phép thay đổi mã bệnh nhân.");
            if (lichHen.MaDichVuYTe != dto.MaDichVuYTe)
                throw new InvalidOperationException("Không được phép thay đổi mã dịch vụ y tế.");

            if (dto.MaNhanVien == 0)
            {
                lichHenUpdate.MaNhanVien = null;
                throw new NotFoundException("Nhân viên không tồn tại");
            }    

            await _nhanVienService.GetByIdAsync((int)dto.MaNhanVien);
            lichHenUpdate.MaNhanVien = dto.MaNhanVien;


            if (dto.MaPhongKham == 0)
            {
                lichHenUpdate.MaPhongKham = null;
                throw new NotFoundException("Lịch hẹn không tồn tại");
            }

            await _phongKhamService.GetByIdAsync((int)dto.MaPhongKham);
            lichHenUpdate.MaPhongKham = dto.MaPhongKham;

            if (dto.NgayHen < DateTime.Now)
                throw new ArgumentException("Ngày hẹn không thể là ngày trong quá khứ.");
            lichHenUpdate.NgayHen = dto.NgayHen;

            lichHenUpdate.TrangThai = dto.TrangThai;

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.UpdateAsync(lichHenUpdate));
        }

        public async Task<LichHenDTO> UpdateTrangThaiAsync(LichHenUpdateDTO dto)
        {
            var lichHen = await GetByIdAsync(dto.MaLichHen);

            if (lichHen.TrangThai == "Hủy")
                throw new InvalidOperationException("Lịch hẹn đã bị hủy, không thể thay đổi trạng thái.");

            if (lichHen.TrangThai.Equals("Đã xác nhận"))
                throw new InvalidOperationException("Lịch hẹn đã xác nhận, không thể thay đổi trạng thái.");
            
            var role = _jwtService.GetCurrentRole();
            if (dto.TrangThai == "Hoàn thành" && role != "BacSi")
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
