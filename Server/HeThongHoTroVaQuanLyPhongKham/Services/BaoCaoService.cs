using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class BaoCaoService : IBaoCaoService
    {
        private readonly IRepository<TblHoaDon> _hoaDonRepository;
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IRepository<TblDonThuoc> _donThuocRepository;
        private readonly IRepository<TblDichVuYTe> _dichVuYTeRepository;
        private readonly IMapper<HoaDonDTO, TblHoaDon> _hoaDonMapping;

        public BaoCaoService(IRepository<TblHoaDon> hoaDonRepository, IRepository<TblLichHen> lichHenRepository, IRepository<TblDonThuoc> donThuocRepository, IRepository<TblDichVuYTe> dichVuYTeRepository, IMapper<HoaDonDTO, TblHoaDon> hoaDonMapping)
        {
            _hoaDonRepository = hoaDonRepository;
            _lichHenRepository = lichHenRepository;
            _donThuocRepository = donThuocRepository;
            _dichVuYTeRepository = dichVuYTeRepository;
            _hoaDonMapping = hoaDonMapping;
        }

        public async Task<DichVuYTeReportDTO> ThongKeDichVuYTeAsync(DateTime tuNgay, DateTime denNgay)
        {
            var lichHens = await _lichHenRepository.GetQueryable()
                .Include(lh => lh.MaDichVuYteNavigation)
                .Where(lh => lh.NgayHen >= tuNgay && lh.NgayHen <= denNgay && lh.MaDichVuYte != null)
                .ToListAsync();

            var report = new DichVuYTeReportDTO
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                TongSoBenhNhan = lichHens.Count
            };

            // Nhóm theo dịch vụ y tế
            var groupedByDichVu = lichHens
                .GroupBy(lh => lh.MaDichVuYteNavigation.Ten)
                .ToDictionary(g => g.Key, g => g.Count());

            report.SoLuongTheoDichVu = groupedByDichVu;

            return report;
        }

        public async Task<DoanhThuReportDTO> ThongKeDoanhThuAsync(DateTime tuNgay, DateTime denNgay, string trangThaiThanhToan = null)
        {
            var query = _hoaDonRepository.GetQueryable()
                .Where(hd => hd.NgayThanhToan >= tuNgay && hd.NgayThanhToan <= denNgay);

            if (!string.IsNullOrEmpty(trangThaiThanhToan))
                query = query.Where(hd => hd.TrangThaiThanhToan == trangThaiThanhToan);

            var hoaDons = await query.ToListAsync();

            var report = new DoanhThuReportDTO
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                TrangThaiThanhToan = trangThaiThanhToan ?? "Tất cả",
                SoHoaDon = hoaDons.Count,
                TongDoanhThu = hoaDons.Sum(hd => hd.TongTien),
                DanhSachHoaDon = hoaDons.Select(hd => _hoaDonMapping.MapEntityToDto(hd)).ToList()
            };

            return report;
        }

        public async Task<DonThuocReportDTO> ThongKeDonThuocAsync(DateTime tuNgay, DateTime denNgay)
        {
            var donThuocs = await _donThuocRepository.GetQueryable()
                .Include(dt => dt.MaHoSoYteNavigation)
                .Where(dt => dt.NgayKeDon >= tuNgay && dt.NgayKeDon <= denNgay)
                .ToListAsync();

            var report = new DonThuocReportDTO
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                TongSoDonThuoc = donThuocs.Count
            };

            // Nhóm theo bệnh nhân
            var groupedByBenhNhan = donThuocs
                .GroupBy(dt => dt.MaHoSoYteNavigation.MaBenhNhan)
                .ToDictionary(g => g.Key, g => g.Count());

            report.SoLuongTheoBenhNhan = groupedByBenhNhan;

            return report;
        }

        public async Task<LichHenReportDTO> ThongKeLichHenAsync(DateTime tuNgay, DateTime denNgay)
        {
            var lichHens = await _lichHenRepository.GetQueryable()
                .Where(lh => lh.NgayHen >= tuNgay && lh.NgayHen <= denNgay)
                .ToListAsync();

            var report = new LichHenReportDTO
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                TongSoLichHen = lichHens.Count
            };

            // Nhóm theo trạng thái
            var groupedByTrangThai = lichHens
                .GroupBy(lh => lh.TrangThai)
                .ToDictionary(g => g.Key, g => g.Count());

            report.SoLuongTheoTrangThai = groupedByTrangThai;

            return report;
        }
    }
}
