using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> _donThuocChiTietMapping;
        private readonly IMapper<TrieuChungDTO, TblTrieuChung> _trieuChungMapping;
        private readonly IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> _ketQuaXetNghiemMapping;
        private readonly IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> _ketQuaDieuTriMapping;
        private readonly IMapper<DonThuocDTO, TblDonThuoc> _donThuocMapping;
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IMapper<LichHenDTO, TblLichHen> _lichHenMapping;

        public HoSoYTeService(IRepository<TblHoSoYTe> hoSoYTeRepository, IMapper<HoSoYTeDTO, TblHoSoYTe> hoSoYTeMapping, IRepository<TblBenhNhan> benhNhanRepository, IKetQuaDieuTriService ketQuaDieuTriService, IDonThuocService donThuocService, ITrieuChungService trieuChungService, IKetQuaXetNghiem ketQuaXetNghiem, IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> donThuocChiTietMapping, IMapper<TrieuChungDTO, TblTrieuChung> trieuChungMapping, IMapper<KetQuaXetNghiemDTO, TblKetQuaXetNghiem> ketQuaXetNghiemMapping, IMapper<KetQuaDieuTriDTO, TblKetQuaDieuTri> ketQuaDieuTriMapping, IMapper<DonThuocDTO, TblDonThuoc> donThuocMapping, IRepository<TblLichHen> lichHenRepository, IMapper<LichHenDTO, TblLichHen> lichHenMapping)
        {
            _hoSoYTeRepository = hoSoYTeRepository;
            _hoSoYTeMapping = hoSoYTeMapping;
            _benhNhanRepository = benhNhanRepository;
            _ketQuaDieuTriService = ketQuaDieuTriService;
            _donThuocService = donThuocService;
            _trieuChungService = trieuChungService;
            _ketQuaXetNghiem = ketQuaXetNghiem;
            _donThuocChiTietMapping = donThuocChiTietMapping;
            _trieuChungMapping = trieuChungMapping;
            _ketQuaXetNghiemMapping = ketQuaXetNghiemMapping;
            _ketQuaDieuTriMapping = ketQuaDieuTriMapping;
            _donThuocMapping = donThuocMapping;
            _lichHenRepository = lichHenRepository;
            _lichHenMapping = lichHenMapping;
        }

        public async Task<HoSoYTeDTO> AddAsync(HoSoYTeDTO dto)
        {
            var benhNhan = await _benhNhanRepository.FindByIdAsync(dto.MaBenhNhan, "MaBenhNhan");
            if (benhNhan is null)
                throw new NotFoundException($"Bệnh nhân với ID [{dto.MaBenhNhan}] không tồn tại.");

            var hsyt = await _hoSoYTeRepository.GetQueryable().FirstOrDefaultAsync(hsyt => hsyt.MaBenhNhan == dto.MaBenhNhan);
            if (hsyt != null && hsyt.MaBenhNhan == dto.MaBenhNhan)
                throw new InvalidOperationException("Bệnh nhân này đã có hồ sơ y tế, chỉ có thể cập nhật");

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

        public async Task<IEnumerable<LichHenDTO>> GetLichHenByMaHoSoYTeAsync(int maHoSoYTe)
        {
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(maHoSoYTe, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{maHoSoYTe}] không tồn tại.");

            var lichHens = await _lichHenRepository.GetQueryable()
                .Where(lh => lh.MaBenhNhan == hoSoYTe.MaBenhNhan)
                .ToListAsync();

            return lichHens.Select(lh => _lichHenMapping.MapEntityToDto(lh));
        }

        public async Task<HoSoYTeDetailDto> GetMedicalRecordDetailAsync(int maHoSoYTe)
        {
            var hoSoYTe = await _hoSoYTeRepository.GetQueryable()
                .Include(h => h.TblTrieuChungs)
                .Include(h => h.TblKetQuaXetNghiems)
                .Include(h => h.TblDonThuocs)
                    .ThenInclude(d => d.TblDonThuocChiTiets)
                    .ThenInclude(dt => dt.MaThuocNavigation)
                .Include(h => h.TblKetQuaDieuTris)
                .FirstOrDefaultAsync(h => h.MaHoSoYte == maHoSoYTe);

            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{maHoSoYTe}] không tồn tại.");

            return new HoSoYTeDetailDto
            {
                MaHoSoYTe = hoSoYTe.MaHoSoYte,
                MaBenhNhan = hoSoYTe.MaBenhNhan,
                ChuanDoan = hoSoYTe.ChuanDoan ?? "Chưa có thông tin chuẩn đoán",
                PhuongPhapDieuTri = hoSoYTe.PhuongPhapDieuTri ?? "Chưa có thông tin phương pháp điều trị",
                LichSuBenh = hoSoYTe.LichSuBenh ?? "Không có thông tin lịch sử bệnh",
                TrieuChung = hoSoYTe.TblTrieuChungs.Select(tc => _trieuChungMapping.MapEntityToDto(tc)).ToList(),
                KetQuaXetNghiem = hoSoYTe.TblKetQuaXetNghiems.Select(kqxn => _ketQuaXetNghiemMapping.MapEntityToDto(kqxn)).ToList(),
                DonThuoc = hoSoYTe.TblDonThuocs.Select(dt => _donThuocMapping.MapEntityToDto(dt)).ToList(),
                KetQuaDieuTri = hoSoYTe.TblKetQuaDieuTris.Select(kqdt => _ketQuaDieuTriMapping.MapEntityToDto(kqdt)).ToList()
            };
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
