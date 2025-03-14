using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class BenhNhanService : BaseService, IService<BenhNhanDTO>
    {
        private readonly IRepository<TblBenhNhan> _benhNhanRepository;
        private readonly IMapper<BenhNhanDTO, TblBenhNhan> _benhNhanMapping;
        private readonly IHoSoYTeService _hoSoYTeService;
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;

        public BenhNhanService(IRepository<TblBenhNhan> benhNhanRepository, IMapper<BenhNhanDTO, TblBenhNhan> benhNhanMapping, IHoSoYTeService hoSoYTeService, IRepository<TblHoSoYTe> hoSoYTeRepository)
        {
            _benhNhanRepository = benhNhanRepository;
            _benhNhanMapping = benhNhanMapping;
            _hoSoYTeService = hoSoYTeService;
            _hoSoYTeRepository = hoSoYTeRepository;
        }

        public async Task<BenhNhanDTO> AddAsync(BenhNhanDTO dto)
        {
            return _benhNhanMapping.MapEntityToDto(
                await _benhNhanRepository.CreateAsync(
                    _benhNhanMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            var benhNhan = await GetByIdAsync(id);

            var hoSoYTeList = await _hoSoYTeRepository.GetQueryable()
                .Where(h => h.MaBenhNhan == id)
                .ToListAsync();

            if (hoSoYTeList.Any())
                foreach (var hoSoYTe in hoSoYTeList)
                    await _hoSoYTeService.DeleteAsync(hoSoYTe.MaHoSoYte);

            await _benhNhanRepository.DeleteAsync(
                _benhNhanMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<BenhNhanDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _benhNhanRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var dichVuYTes = await _benhNhanRepository.FindAllAsync(page, pageSize, pageSkip, "MaBenhNhan");
            var dtoList = dichVuYTes.Select(t => _benhNhanMapping.MapEntityToDto(t));

            return (dtoList, totalItems, totalPages);
        }

        public async Task<BenhNhanDTO> GetByIdAsync(int id)
        {
            var benhNhan = await _benhNhanRepository.FindByIdAsync(id, "MaBenhNhan");
            if (benhNhan is null)
                throw new NotFoundException($"Bệnh nhân với ID [{id}] không tồn tại.");

            return _benhNhanMapping.MapEntityToDto(benhNhan);
        }

        public async Task<BenhNhanDTO> UpdateAsync(BenhNhanDTO dto)
        {
            var benhNhanUpdate = _benhNhanMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaBenhNhan));

            _benhNhanMapping.MapDtoToEntity(dto, benhNhanUpdate);

            return _benhNhanMapping.MapEntityToDto(
                await _benhNhanRepository.UpdateAsync(benhNhanUpdate));
        }
    }
}
