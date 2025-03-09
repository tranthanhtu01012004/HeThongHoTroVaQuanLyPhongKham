using HeThongHoTroVaQuanLyPhongKham.Dtos;
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

        public HoaDonService(IRepository<TblHoaDon> hoaDonRepository, IMapper<HoaDonDTO, TblHoaDon> hoaDonMapping)
        {
            _hoaDonRepository = hoaDonRepository;
            _hoaDonMapping = hoaDonMapping;
        }

        public async Task<HoaDonDTO> AddAsync(HoaDonDTO dto)
        {
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

        public async Task<IEnumerable<HoaDonDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var hoaDons = await _hoaDonRepository.FindAllAsync(page, pageSize, pageSkip, "MaHoaDon");
            return hoaDons.Select(t => _hoaDonMapping.MapEntityToDto(t));
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

            _hoaDonMapping.MapDtoToEntity(dto, hoaDonUpdate);

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(hoaDonUpdate));
        }

        public async Task<HoaDonDTO> UpdateTongTienAsync(int maHoaDon, HoaDonUpdateDTO dto)
        {
            var hoaDon = await GetByIdAsync(maHoaDon);

            hoaDon.TongTien = dto.TongTien;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(
                    _hoaDonMapping.MapDtoToEntity(hoaDon)));
        }

        public async Task<HoaDonDTO> UpdateTrangThaiAsync(int maHoaDon, HoaDonUpdateDTO dto)
        {
            var hoaDon = await GetByIdAsync(maHoaDon);
            
            if (dto.TrangThaiThanhToan != null)
                hoaDon.TrangThaiThanhToan = dto.TrangThaiThanhToan;

            return _hoaDonMapping.MapEntityToDto(
                await _hoaDonRepository.UpdateAsync(
                    _hoaDonMapping.MapDtoToEntity(hoaDon)));
        }
    }
}
