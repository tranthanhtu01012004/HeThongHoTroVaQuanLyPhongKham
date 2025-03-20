using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class HoaDonService : BaseService, IHoaDonService
    {
        private readonly IRepository<TblHoaDon> _hoaDonRepository;
        private readonly IMapper<HoaDonDTO, TblHoaDon> _hoaDonMapping;
        //private readonly ILichHenService _lichHenService;
        private readonly IRepository<TblLichHen> _lichHenRepository;

        public HoaDonService(IRepository<TblHoaDon> hoaDonRepository, IMapper<HoaDonDTO, TblHoaDon> hoaDonMapping, IRepository<TblLichHen> lichHenRepository)
        {
            _hoaDonRepository = hoaDonRepository;
            _hoaDonMapping = hoaDonMapping;
            _lichHenRepository = lichHenRepository;
        }

        public async Task<HoaDonDTO> AddAsync(HoaDonDTO dto)
        {
            var lichHen = await _lichHenRepository.FindByIdAsync(dto.MaLichHen, "MaLichHen");
            if (lichHen is null)
                throw new NotFoundException("Lịch hẹn không tồn tại");

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.CreateAsync(
                    _hoaDonMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _hoaDonRepository.DeleteAsync(
                _hoaDonMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<HoaDonDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _hoaDonRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var hoaDons = await _hoaDonRepository.FindAllAsync(page, pageSize, pageSkip, "MaDonThuoc");
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

            hoaDon.TrangThaiThanhToan = dto.TrangThaiThanhToan;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(
                    _hoaDonMapping.MapDtoToEntity(hoaDon)));
        }
    }
}
