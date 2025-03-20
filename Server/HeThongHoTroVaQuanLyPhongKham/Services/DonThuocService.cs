using System.Linq;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLydonThuoc.Services
{
    public class DonThuocService : BaseService, IDonThuocService
    {
        private readonly IRepository<TblDonThuoc> _donThuocRepository;
        private readonly IMapper<DonThuocDTO, TblDonThuoc> _donThuocMapping;
        //private readonly IService<HoSoYTeDTO> _hoSoYTeService; -> loi Circular Dependency Detection
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;
        private readonly IThuocService _thuocService;
        private readonly IKetQuaDieuTriService _ketQuaDieuTriService;
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IRepository<TblHoaDon> _hoaDonRepository;

        public DonThuocService(IRepository<TblDonThuoc> donThuocRepository, IMapper<DonThuocDTO, TblDonThuoc> donThuocMapping, IRepository<TblHoSoYTe> hoSoYTeRepository, IThuocService thuocService, IKetQuaDieuTriService ketQuaDieuTriService, IRepository<TblLichHen> lichHenRepository, IRepository<TblHoaDon> hoaDonRepository)
        {
            _donThuocRepository = donThuocRepository;
            _donThuocMapping = donThuocMapping;
            _hoSoYTeRepository = hoSoYTeRepository;
            _thuocService = thuocService;
            _ketQuaDieuTriService = ketQuaDieuTriService;
            _lichHenRepository = lichHenRepository;
            _hoaDonRepository = hoaDonRepository;
        }

        public async Task<DonThuocDTO> AddAsync(DonThuocDTO dto)
        {
            // Kiểm tra hồ sơ y tế
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            // Kiểm tra danh sách thuốc hợp lệ
            var danhSachThuocHopLe = await _thuocService.GetAllAsync();
            foreach (var chiTiet in dto.ChiTietThuocList)
            {
                if (!danhSachThuocHopLe.Any(thuoc => thuoc.MaThuoc == chiTiet.MaThuoc))
                    throw new NotFoundException($"Mã thuốc với ID [{chiTiet.MaThuoc}] không tồn tại.");
            }

            // Kiểm tra trùng lặp đơn thuốc
            // Lấy danh sách đơn thuốc có cùng MaHoSoYte và NgayKeDon
            var donThuocList = await _donThuocRepository.GetQueryable()
                .Include(dt => dt.TblDonThuocChiTiets)
                .Where(dt => dt.MaHoSoYte == dto.MaHoSoYte && dt.NgayKeDon == dto.NgayKeDon)
                .ToListAsync();

            // So sánh danh sách chi tiết thuốc
            var existingDonThuoc = donThuocList.FirstOrDefault(dt =>
                dt.TblDonThuocChiTiets.Count == dto.ChiTietThuocList.Count &&
                dt.TblDonThuocChiTiets.All(ct =>
                    dto.ChiTietThuocList.Any(dtoCt =>
                        dtoCt.MaThuoc == ct.MaThuoc &&
                        dtoCt.SoLuong == ct.SoLuong &&
                        dtoCt.CachDung == ct.CachDung &&
                        dtoCt.LieuLuong == ct.LieuLuong &&
                        dtoCt.TanSuat == ct.TanSuat)));

            if (existingDonThuoc != null)
                throw new InvalidOperationException("Đơn thuốc này đã tồn tại trong hệ thống.");

            // Kiểm tra xem có lịch hẹn "Đã hoàn thành" nào liên quan đến maHoSoYTe không
            var lichHen = await _lichHenRepository.GetQueryable()
                .Where(lh => lh.MaBenhNhan == hoSoYTe.MaBenhNhan && lh.TrangThai == "Đã hoàn thành")
                .OrderByDescending(lh => lh.NgayHen)
                .FirstOrDefaultAsync();

            if (lichHen != null)
            {
                var hoaDon = await _hoaDonRepository.GetQueryable()
                    .Where(hd => hd.MaLichHen == lichHen.MaLichHen)
                    .FirstOrDefaultAsync();
                if (hoaDon != null)
                    dto.MaHoaDon = hoaDon.MaHoaDon;
            }

            // Thêm đơn thuốc mới
            return _donThuocMapping.MapEntityToDto(
                await _donThuocRepository.CreateAsync(
                    _donThuocMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            var donThuoc = await GetByIdAsync(id);
           
            await _ketQuaDieuTriService.DeleteByMaHoSoYTeAsync(donThuoc.MaHoSoYte);

            await _donThuocRepository.DeleteAsync(
                _donThuocMapping.MapDtoToEntity(
                    donThuoc));
        }

        public async Task DeleteByMaHoSoYTeAsync(int id)
        {
            var donthuocs = await _donThuocRepository.GetQueryable()
                                .Where(dt => dt.MaHoSoYte == id)
                                .ToListAsync();
            if (donthuocs.Any())
                await _donThuocRepository.DeleteAsync(donthuocs);
        }

        public async Task<(IEnumerable<DonThuocDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _donThuocRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var query = _donThuocRepository.GetQueryable();
            query = query.Include(d => d.TblDonThuocChiTiets);

            var donThuocs = await _donThuocRepository.FindAllWithQueryAsync(query, page, pageSize, pageSkip, "MaDonThuoc");
            var dtoList = donThuocs.Select(t => _donThuocMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<DonThuocDTO> GetByIdAsync(int id)
        {
            var query = _donThuocRepository.GetQueryable()
                .Include(d => d.TblDonThuocChiTiets).AsQueryable().AsNoTracking();

            var donThuoc = await _donThuocRepository.FindByIdWithQueryAsync(query, id, "MaDonThuoc");
            if (donThuoc is null)
                throw new NotFoundException($"Đơn thuốc với ID [{id}] không tồn tại.");

            return _donThuocMapping.MapEntityToDto(donThuoc);
        }

        public async Task<DonThuocDTO> UpdateAsync(DonThuocDTO dto)
        {
            var donThuocUpdate = _donThuocMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaDonThuoc));

            _donThuocMapping.MapDtoToEntity(dto, donThuocUpdate);

            return _donThuocMapping.MapEntityToDto(
                await _donThuocRepository.UpdateAsync(donThuocUpdate));
        }
        public async Task<IEnumerable<DonThuocDTO>> GetByMaHoSoYTeAsync(int maHoSoYTe)
        {
            var query = _donThuocRepository.GetQueryable()
                .Include(d => d.TblDonThuocChiTiets)
                .Where(dt => dt.MaHoSoYte == maHoSoYTe);

            var donThuocs = await query.ToListAsync();
            return donThuocs.Select(dt => _donThuocMapping.MapEntityToDto(dt));
        }
    }
}
