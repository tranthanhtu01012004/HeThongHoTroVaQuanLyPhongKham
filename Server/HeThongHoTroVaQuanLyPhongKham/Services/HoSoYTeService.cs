using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class HoSoYTeService : BaseService, IHoSoYTeService
    {
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;
        private readonly IMapper<HoSoYTeDTO, TblHoSoYTe> _hoSoYTeMapping;
        //private readonly IService<BenhNhanDTO> _benhNhanService;
        private readonly IRepository<TblBenhNhan> _benhNhanRepository; // Circular denpendency
        private readonly IKetQuaDieuTriService _ketQuaDieuTriService;
        private readonly IDonThuocService _donThuocService;
        private readonly ITrieuChungService _trieuChungService;
        private readonly IKetQuaXetNghiem _ketQuaXetNghiem;

        public HoSoYTeService(IRepository<TblHoSoYTe> hoSoYTeRepository, IMapper<HoSoYTeDTO, TblHoSoYTe> hoSoYTeMapping, IRepository<TblBenhNhan> benhNhanRepository, IKetQuaDieuTriService ketQuaDieuTriService, IDonThuocService donThuocService, ITrieuChungService trieuChungService, IKetQuaXetNghiem ketQuaXetNghiem)
        {
            _hoSoYTeRepository = hoSoYTeRepository;
            _hoSoYTeMapping = hoSoYTeMapping;
            _benhNhanRepository = benhNhanRepository;
            _ketQuaDieuTriService = ketQuaDieuTriService;
            _donThuocService = donThuocService;
            _trieuChungService = trieuChungService;
            _ketQuaXetNghiem = ketQuaXetNghiem;
        }

        public async Task<HoSoYTeDTO> AddAsync(HoSoYTeDTO dto)
        {
            var benhNhan = await _benhNhanRepository.FindByIdAsync(dto.MaBenhNhan, "MaBenhNhan");
            if (benhNhan is null)
                throw new NotFoundException($"Bệnh nhân với ID [{dto.MaBenhNhan}] không tồn tại.");

            return _hoSoYTeMapping.MapEntityToDto(
                await _hoSoYTeRepository.CreateAsync(
                    _hoSoYTeMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _ketQuaDieuTriService.DeleteByMaHoSoYTeAsync(id);
            await _donThuocService.DeleteByMaHoSoYTeAsync(id);
            await _trieuChungService.DeleteByMaHoSoYTeAsync(id);
            await _ketQuaXetNghiem.DeleteByMaHoSoYTeAsync(id);

            await _hoSoYTeRepository.DeleteAsync(
                _hoSoYTeMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<HoSoYTeDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _hoSoYTeRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var hoSoYTe = await _hoSoYTeRepository.FindAllAsync(page, pageSize, pageSkip, "MaHoSoYte");
            var dtoList = hoSoYTe.Select(t => _hoSoYTeMapping.MapEntityToDto(t));

            return (dtoList, totalItems, totalPages);
        }

        public async Task<HoSoYTeDTO> GetByIdAsync(int id)
        {
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(id, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{id}] không tồn tại.");

            return _hoSoYTeMapping.MapEntityToDto(hoSoYTe);
        }

        public async Task<HoSoYTeDTO> UpdateAsync(HoSoYTeDTO dto)
        {
            var hoSoYTeUpdate = _hoSoYTeMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaHoSoYTe));

            var benhNhan = await _benhNhanRepository.FindByIdAsync(dto.MaBenhNhan, "MaBenhNhan");
            if (benhNhan is null)
                throw new NotFoundException($"Bệnh nhân với ID [{dto.MaBenhNhan}] không tồn tại.");

            _hoSoYTeMapping.MapDtoToEntity(dto, hoSoYTeUpdate);

            return _hoSoYTeMapping.MapEntityToDto(
                await _hoSoYTeRepository.UpdateAsync(hoSoYTeUpdate));
        }

        public async Task<HoSoYTeDTO> UpdateChuanDoanAsync(HoSoYTeUpdateDTO dto)
        {
            var hoSoYTe = await GetByIdAsync(dto.MaHoSoYTe);
            if (dto.ChuanDoan is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYTe}] không có giá trị chuẩn đoán.");

            hoSoYTe.ChuanDoan = dto.ChuanDoan;

            return _hoSoYTeMapping.MapEntityToDto(
                await _hoSoYTeRepository.UpdateAsync(
                    _hoSoYTeMapping.MapDtoToEntity(hoSoYTe)));
        }

        public async Task<HoSoYTeDTO> UpdatePhuongPhapDieuTriAsync(HoSoYTeUpdateDTO dto)
        {
            var hoSoYTe = await GetByIdAsync(dto.MaHoSoYTe);
            if (dto.PhuongPhapDieuTri is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYTe}] không có giá trị phương pháp điều trị.");

            hoSoYTe.PhuongPhapDieuTri = dto.PhuongPhapDieuTri;

            return _hoSoYTeMapping.MapEntityToDto(
                await _hoSoYTeRepository.UpdateAsync(
                    _hoSoYTeMapping.MapDtoToEntity(hoSoYTe)));
        }
    }
}
