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
    public class HoaDonService : BaseService, IHoaDonService
    {
        private readonly IRepository<TblHoaDon> _hoaDonRepository;
        private readonly IMapper<HoaDonDTO, TblHoaDon> _hoaDonMapping;
        //private readonly ILichHenService _lichHenService;
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IRepository<TblDonThuoc> _donThuocRepository;

        public HoaDonService(IRepository<TblHoaDon> hoaDonRepository, IMapper<HoaDonDTO, TblHoaDon> hoaDonMapping, IRepository<TblLichHen> lichHenRepository, IRepository<TblDonThuoc> donThuocRepository)
        {
            _hoaDonRepository = hoaDonRepository;
            _hoaDonMapping = hoaDonMapping;
            _lichHenRepository = lichHenRepository;
            _donThuocRepository = donThuocRepository;
        }

        public async Task<HoaDonDTO> AddAsync(HoaDonDTO dto)
        {
            if (dto.MaLichHen.HasValue)
            {
                var lichHen = await _lichHenRepository.FindByIdAsync(dto.MaLichHen.Value, "MaLichHen");
                if (lichHen is null)
                    throw new NotFoundException($"Lịch hẹn với ID [{dto.MaLichHen.Value}] không tồn tại.");
            }

            dto.MaHoaDon = 0;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.CreateAsync(
                    _hoaDonMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            var hoaDon = await _hoaDonRepository.FindByIdAsync(id, "MaHoaDon");
            if (hoaDon == null)
                throw new NotFoundException($"Hóa đơn với ID [{id}] không tồn tại.");

            var donThuocs = await _donThuocRepository.GetQueryable()
                .Where(dt => dt.MaHoaDon == id)
                .ToListAsync();

            foreach (var donThuoc in donThuocs)
            {
                donThuoc.MaHoaDon = null;
                await _donThuocRepository.UpdateAsync(donThuoc);
            }

            await _hoaDonRepository.DeleteAsync(hoaDon);
        }

        public async Task<(IEnumerable<HoaDonDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _hoaDonRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var hoaDons = await _hoaDonRepository.FindAllAsync(page, pageSize, pageSkip, "MaHoaDon");
            var dtoList = hoaDons.Select(t => _hoaDonMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<HoaDonDTO> GetByIdAsync(int id)
        {
            var hoaDon = await _hoaDonRepository.FindByIdAsync(id, "MaHoaDon");
            if (hoaDon is null)
                throw new NotFoundException($"Hóa đơn với ID [{id}] không tồn tại.");

            return _hoaDonMapping.MapEntityToDto(hoaDon);
        }

        public async Task<HoaDonDTO> UpdateAsync(HoaDonDTO dto)
        {
            var hoaDonUpdate = _hoaDonMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaHoaDon));

            //await _lichHenService.GetByIdAsync(dto.MaLichHen);

            _hoaDonMapping.MapDtoToEntity(dto, hoaDonUpdate);

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(hoaDonUpdate));
        }

        public async Task<HoaDonDTO> UpdateTongTienAsync(HoaDonUpdateDTO dto)
        {
            var hoaDon = await GetByIdAsync(dto.MaHoaDon);

            if (dto.TongTien is null)
                throw new NotFoundException($"Hóa đơn với ID [{dto.MaHoaDon}] không có giá trị tổng tiền.");

            hoaDon.TongTien = (decimal) dto.TongTien;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(
                    _hoaDonMapping.MapDtoToEntity(hoaDon)));
        }

        public async Task<HoaDonDTO> UpdateTrangThaiAsync(HoaDonUpdateDTO dto)
        {
            var hoaDon = await GetByIdAsync(dto.MaHoaDon);
            
            if (dto.TrangThaiThanhToan is null)
                throw new NotFoundException($"Hóa đơn với ID [{dto.MaHoaDon}] không có giá trị trạng thái thanh toán.");

            if (hoaDon.TrangThaiThanhToan.Equals("Đã thanh toán"))
                throw new UnauthorizedAccessException("Hóa đơn đã thanh toán, không thể thay đổi trạng thái.");

            hoaDon.TrangThaiThanhToan = dto.TrangThaiThanhToan;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(
                    _hoaDonMapping.MapDtoToEntity(hoaDon)));
        }
    }
}
