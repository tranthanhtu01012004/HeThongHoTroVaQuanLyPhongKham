using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services;

namespace HeThongHoTroVaQuanLydonThuoc.Services
{
    public class DonThuocService: BaseService, IDonThuocService
    {
        private readonly IRepository<TblDonThuoc> _donThuocRepository;
        private readonly IMapper<DonThuocDTO, TblDonThuoc> _donThuocMapping;

        public DonThuocService(IRepository<TblDonThuoc> donThuocRepository, IMapper<DonThuocDTO, TblDonThuoc> donThuocMapping)
        {
            _donThuocRepository = donThuocRepository;
            _donThuocMapping = donThuocMapping;
        }

        public async Task<DonThuocDTO> AddAsync(DonThuocDTO dto)
        {
            return _donThuocMapping.MapEntityToDto(
                await _donThuocRepository.CreateAsync(
                    _donThuocMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _donThuocRepository.DeleteAsync(
                _donThuocMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<DonThuocDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var donThuocs = await _donThuocRepository.FindAllAsync(page, pageSize, pageSkip, "MaDonThuoc");
            return donThuocs.Select(t => _donThuocMapping.MapEntityToDto(t));
        }

        public async Task<DonThuocDTO> GetByIdAsync(int id)
        {
            var donThuoc = await _donThuocRepository.FindByIdAsync(id, "MaDonThuoc");
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
    }
}
