using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class DichVuYTeService : BaseService, IService<DichVuYTeDTO>
    {
        private readonly IRepository<TblDichVuYTe> _dichVuYTeRepository;
        private readonly IMapper<DichVuYTeDTO, TblDichVuYTe> _dichVuYTeMapping;
        private readonly IRepository<TblHoaDon> _hoaDonRepository;
        private readonly ILichHenService _lichHenService;
        private readonly IRepository<TblLichHen> _lichHenRepository;

        public DichVuYTeService(IRepository<TblDichVuYTe> dichVuYTeRepository, IMapper<DichVuYTeDTO, TblDichVuYTe> dichVuYTeMapping, IRepository<TblHoaDon> hoaDonRepository, ILichHenService lichHenService, IRepository<TblLichHen> lichHenRepository)
        {
            _dichVuYTeRepository = dichVuYTeRepository;
            _dichVuYTeMapping = dichVuYTeMapping;
            _hoaDonRepository = hoaDonRepository;
            _lichHenService = lichHenService;
            _lichHenRepository = lichHenRepository;
        }

        public async Task<DichVuYTeDTO> AddAsync(DichVuYTeDTO dto)
        {
            return _dichVuYTeMapping.MapEntityToDto(
                await _dichVuYTeRepository.CreateAsync(
                    _dichVuYTeMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            //var dichVuYTe = await GetByIdAsync(id);
            //if (dichVuYTe is null)
            //    throw new Exception("Dịch vụ y tế không tồn tại.");
            //var lichHens = await _lichHenRepository.GetQueryable()
            //    .Where(lh => lh.MaDichVuYte == id && lh.TrangThai != "Hủy")
            //    .ToListAsync();

            //if (lichHens.Any())
            //{
            //    foreach (var lichHen in lichHens)
            //    {
            //        lichHen.MaDichVuYte = 0;
            //        await _lichHenRepository.UpdateAsync(lichHen);
            //    }
            //}

            await _dichVuYTeRepository.DeleteAsync(
                _dichVuYTeMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<DichVuYTeDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _dichVuYTeRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);

            var dichVuYTes = await _dichVuYTeRepository.FindAllAsync(page, pageSize, pageSkip, "MaDichVuYte");
            var dtoList = dichVuYTes.Select(t => _dichVuYTeMapping.MapEntityToDto(t));

            return (dtoList, totalItems, totalPages);
        }

        public async Task<DichVuYTeDTO> GetByIdAsync(int id)
        {
            var dichVuYTe = await _dichVuYTeRepository.FindByIdAsync(id, "MaDichVuYte");
            if (dichVuYTe is null)
                throw new NotFoundException($"Dịch vụ y tế với ID [{id}] không tồn tại.");

            return _dichVuYTeMapping.MapEntityToDto(dichVuYTe);
        }

        public async Task<DichVuYTeDTO> UpdateAsync(DichVuYTeDTO dto)
        {
            var dichVuYTeUpdate = _dichVuYTeMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaDichVuYTe));

            _dichVuYTeMapping.MapDtoToEntity(dto, dichVuYTeUpdate);

            return _dichVuYTeMapping.MapEntityToDto(
                await _dichVuYTeRepository.UpdateAsync(dichVuYTeUpdate));
        }
    }
}
