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
        private readonly IService<BenhNhanDTO> _benhNhanService;
        private readonly IKetQuaDieuTriService _ketQuaDieuTriService;
        private readonly IDonThuocService _donThuocService;

        public HoSoYTeService(IRepository<TblHoSoYTe> hoSoYTeRepository, IMapper<HoSoYTeDTO, TblHoSoYTe> hoSoYTeMapping, IService<BenhNhanDTO> benhNhanService, IKetQuaDieuTriService ketQuaDieuTriService, IDonThuocService donThuocService)
        {
            _hoSoYTeRepository = hoSoYTeRepository;
            _hoSoYTeMapping = hoSoYTeMapping;
            _benhNhanService = benhNhanService;
            _ketQuaDieuTriService = ketQuaDieuTriService;
            _donThuocService = donThuocService;
        }

        public async Task<HoSoYTeDTO> AddAsync(HoSoYTeDTO dto)
        {
            await _benhNhanService.GetByIdAsync(dto.MaBenhNhan);

            return _hoSoYTeMapping.MapEntityToDto(
                await _hoSoYTeRepository.CreateAsync(
                    _hoSoYTeMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _ketQuaDieuTriService.DeleteByMaHoSoYTeAsync(id);
            await _donThuocService.DeleteByMaHoSoYTeAsync(id);

            await _hoSoYTeRepository.DeleteAsync(
                _hoSoYTeMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<HoSoYTeDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var hoSoYTe = await _hoSoYTeRepository.FindAllAsync(page, pageSize, pageSkip, "MaHoSoYte");
            return hoSoYTe.Select(t => _hoSoYTeMapping.MapEntityToDto(t));
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

            await _benhNhanService.GetByIdAsync(dto.MaBenhNhan);

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
