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
        //private readonly IService<DichVuYTeDTO> _dichVuYTeService;
        private readonly IRepository<TblDichVuYTe> _dichVuYTeRepository;
        private readonly IService<PhongKhamDTO> _phongKhamService;
        private readonly IJwtService _jwtService;
        private readonly IRepository<TblHoaDon> _hoaDonRepository;
        private readonly IRepository<TblDonThuoc> _donThuocRepository;
        private readonly IHoaDonService _hoaDonService;
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;
        private readonly IMapper<HoaDonDTO, TblHoaDon> _hoaDonMapping;

        public LichHenService(IRepository<TblLichHen> lichHenRepository, IMapper<LichHenDTO, TblLichHen> lichHenMapping, IRepository<TblBenhNhan> benhNhanRepository, IService<NhanVienDTO> nhanVienService, IRepository<TblDichVuYTe> dichVuYTeRepository, IService<PhongKhamDTO> phongKhamService, IJwtService jwtService, IRepository<TblHoaDon> hoaDonRepository, IRepository<TblDonThuoc> donThuocRepository, IHoaDonService hoaDonService, IRepository<TblHoSoYTe> hoSoYTeRepository, IMapper<HoaDonDTO, TblHoaDon> hoaDonMapping)
        {
            _lichHenRepository = lichHenRepository;
            _lichHenMapping = lichHenMapping;
            _benhNhanRepository = benhNhanRepository;
            _nhanVienService = nhanVienService;
            _dichVuYTeRepository = dichVuYTeRepository;
            _phongKhamService = phongKhamService;
            _jwtService = jwtService;
            _hoaDonRepository = hoaDonRepository;
            _donThuocRepository = donThuocRepository;
            _hoaDonService = hoaDonService;
            _hoSoYTeRepository = hoSoYTeRepository;
            _hoaDonMapping = hoaDonMapping;
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
            var dichVuYTe = await _dichVuYTeRepository.FindByIdAsync(dto.MaDichVuYTe, "MaDichVuYte");
            if (dichVuYTe is null)
                throw new NotFoundException($"Dịch vụ y tế với ID [{dto.MaDichVuYTe}] không tồn tại.");

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
            if (lichHen is null)
                throw new Exception("Lịch hẹn không tồn tại.");

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

            if (lichHen.TrangThai.Equals("Đã xác nhận") && dto.TrangThai != "Đã hoàn thành")
                throw new InvalidOperationException("Lịch hẹn đã xác nhận, chỉ có thể chuyển sang đã hoàn thành.");

            if (lichHen.TrangThai.Equals("Đã hoàn thành"))
                throw new UnauthorizedAccessException("Lịch hẹn đã hoàn thành, không thể thay đổi trạng thái.");

            var role = _jwtService.GetCurrentRole();
            if (dto.TrangThai.Equals("Đã hoàn thành") && role != "BacSi")
                throw new UnauthorizedAccessException("Chỉ bác sĩ mới được đánh dấu lịch hẹn là hoàn thành.");

            lichHen.TrangThai = dto.TrangThai;

            var updatedLichHen = await _lichHenRepository.UpdateAsync(_lichHenMapping.MapDtoToEntity(lichHen));

            // Tạo hoặc cập nhật hóa đơn khi lịch hẹn hoàn thành
            if (dto.TrangThai.Equals("Đã hoàn thành"))
            {
                // Kiểm tra xem đã có hóa đơn nào liên quan đến lịch hẹn chưa
                var existingHoaDon = await _hoaDonRepository.GetQueryable()
                    .Where(hd => hd.MaLichHen == dto.MaLichHen)
                    .FirstOrDefaultAsync();

                HoaDonDTO createdHoaDon;
                if (existingHoaDon != null)
                {
                    // Có hóa đơn trước đó, kiểm tra trạng thái thanh toán
                    if (existingHoaDon.TrangThaiThanhToan == "Đã thanh toán")
                    {
                        // Trường hợp 2 & 4: Hóa đơn đã thanh toán -> Tạo hóa đơn mới
                        var hoaDonDto = new HoaDonDTO
                        {
                            MaHoaDon = 0,
                            MaLichHen = dto.MaLichHen,
                            TongTien = await CalculateTongTien(dto.MaLichHen),
                            TrangThaiThanhToan = "Chưa thanh toán",
                            NgayThanhToan = null
                        };
                        createdHoaDon = await _hoaDonService.AddAsync(hoaDonDto);
                    }
                    else
                    {
                        // Trường hợp 3: Hóa đơn chưa thanh toán -> Cộng dồn
                        var hoaDonDtoToUpdate = _hoaDonMapping.MapEntityToDto(existingHoaDon);
                        hoaDonDtoToUpdate.TongTien += await CalculateTongTien(dto.MaLichHen);
                        var updatedHoaDon = _hoaDonMapping.MapDtoToEntity(hoaDonDtoToUpdate);
                        await _hoaDonRepository.UpdateAsync(updatedHoaDon);
                        createdHoaDon = hoaDonDtoToUpdate;
                    }
                }
                else
                {
                    // Không có hóa đơn trước đó, tạo hóa đơn mới
                    var hoaDonDto = new HoaDonDTO
                    {
                        MaHoaDon = 0,
                        MaLichHen = dto.MaLichHen,
                        TongTien = await CalculateTongTien(dto.MaLichHen),
                        TrangThaiThanhToan = "Chưa thanh toán",
                        NgayThanhToan = null
                    };
                    createdHoaDon = await _hoaDonService.AddAsync(hoaDonDto);
                }

                // Lấy maHoSoYTe từ maBenhNhan của lịch hẹn
                var maHoSoYTe = await _hoSoYTeRepository.GetQueryable()
                    .Where(hs => hs.MaBenhNhan == lichHen.MaBenhNhan)
                    .Select(hs => hs.MaHoSoYte)
                    .FirstOrDefaultAsync();

                if (maHoSoYTe == 0)
                    throw new NotFoundException($"Không tìm thấy hồ sơ y tế cho bệnh nhân với mã {lichHen.MaBenhNhan}.");

                // Cập nhật tất cả đơn thuốc liên quan đến maHoSoYTe
                var donThuocs = await _donThuocRepository.GetQueryable()
                    .Where(dt => dt.MaHoSoYte == maHoSoYTe)
                    .ToListAsync();

                if (donThuocs.Any())
                {
                    foreach (var donThuoc in donThuocs)
                    {
                        donThuoc.MaHoaDon = createdHoaDon.MaHoaDon;
                        await _donThuocRepository.UpdateAsync(donThuoc);
                    }
                }
            }

            return _lichHenMapping.MapEntityToDto(updatedLichHen);
        }

        Task<(IEnumerable<LichHenDTO> Items, int TotalItems, int TotalPages)> IService<LichHenDTO>.GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        private async Task<decimal> CalculateTongTien(int maLichHen)
        {
            // Lấy thông tin lịch hẹn
            var lichHen = await GetByIdAsync(maLichHen);
            if (lichHen == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn với mã {maLichHen}.");

            // Lấy chi phí dịch vụ y tế
            var dichVuYTe = await _dichVuYTeRepository.FindByIdAsync(lichHen.MaDichVuYTe ?? 0, "MaDichVuYte");
            decimal tongTien = dichVuYTe?.ChiPhi ?? 0;
            Console.WriteLine($"Chi phí dịch vụ y tế: {tongTien}");

            // Lấy maHoSoYTe từ maBenhNhan của lịch hẹn
            var maHoSoYTe = await _hoSoYTeRepository.GetQueryable()
                .Where(hs => hs.MaBenhNhan == lichHen.MaBenhNhan)
                .Select(hs => hs.MaHoSoYte)
                .FirstOrDefaultAsync();

            if (maHoSoYTe == 0)
            {
                Console.WriteLine($"Không tìm thấy maHoSoYTe cho bệnh nhân {lichHen.MaBenhNhan}.");
                return tongTien;
            }

            // Lấy tất cả đơn thuốc liên quan đến maHoSoYTe
            var donThuocs = await _donThuocRepository.GetQueryable()
                .Include(dt => dt.TblDonThuocChiTiets)
                .Where(dt => dt.MaHoSoYte == maHoSoYTe)
                .ToListAsync();

            if (donThuocs.Any())
            {
                decimal tongTienDonThuoc = 0;
                foreach (var donThuoc in donThuocs)
                {
                    if (donThuoc.TblDonThuocChiTiets != null && donThuoc.TblDonThuocChiTiets.Any())
                    {
                        decimal thanhTienDonThuoc = donThuoc.TblDonThuocChiTiets.Sum(ct => ct.ThanhTien);
                        tongTienDonThuoc += thanhTienDonThuoc;
                        Console.WriteLine($"Đơn thuốc {donThuoc.MaDonThuoc}: Thành tiền = {thanhTienDonThuoc}");
                    }
                    else
                    {
                        Console.WriteLine($"Đơn thuốc {donThuoc.MaDonThuoc}: Không có chi tiết đơn thuốc.");
                    }
                }
                tongTien += tongTienDonThuoc;
                Console.WriteLine($"Tổng tiền đơn thuốc: {tongTienDonThuoc}");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy đơn thuốc nào cho maHoSoYTe = {maHoSoYTe}.");
            }

            Console.WriteLine($"Tổng tiền cuối cùng: {tongTien}");
            return tongTien;
        }
    }
}
