using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLydonThuoc.Services
{
    public class DonThuocService : BaseService, IService<DonThuocDTO>
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
            var query = _donThuocRepository.GetQueryable();
            query = query.Include(d => d.TblDonThuocChiTiets);

            var pageSkip = CalculatePageSkip(page, pageSize);
            var donThuocs = await _donThuocRepository.FindAllWithQueryAsync(query, page, pageSize, pageSkip, "MaDonThuoc");
            return donThuocs.Select(t => _donThuocMapping.MapEntityToDto(t));
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

        public Task<DonThuocDTO> UpdateAsync(DonThuocDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
