using System.Linq;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet;
using HeThongHoTroVaQuanLyPhongKham.Services.KetQuaDieuTri;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLydonThuoc.Services
{
    public class DonThuocService : BaseService, IDonThuocService
    {
        private readonly IRepository<TblDonThuoc> _donThuocRepository;
        private readonly IMapper<DonThuocDTO, TblDonThuoc> _donThuocMapping;
        //private readonly IService<HoSoYTeDTO> _hoSoYTeService; -> loi Circular Dependency Detection
        private readonly IRepository<TblHoSoYTe> _hoSoYTeRepository;
        private readonly IThuocService _thuocService;
        private readonly IKetQuaDieuTriService _ketQuaDieuTriService;

        public DonThuocService(IRepository<TblDonThuoc> donThuocRepository, IMapper<DonThuocDTO, TblDonThuoc> donThuocMapping, IRepository<TblHoSoYTe> hoSoYTeRepository, IThuocService thuocService, IKetQuaDieuTriService ketQuaDieuTriService)
        {
            _donThuocRepository = donThuocRepository;
            _donThuocMapping = donThuocMapping;
            _hoSoYTeRepository = hoSoYTeRepository;
            _thuocService = thuocService;
            _ketQuaDieuTriService = ketQuaDieuTriService;
        }

        public async Task<DonThuocDTO> AddAsync(DonThuocDTO dto)
        {
            var hoSoYTe = await _hoSoYTeRepository.FindByIdAsync(dto.MaHoSoYte, "MaHoSoYte");
            if (hoSoYTe is null)
                throw new NotFoundException($"Hồ sơ y tế với ID [{dto.MaHoSoYte}] không tồn tại.");

            var danhSachThuocHopLe = await _thuocService.GetAllAsync();
            foreach (var chiTiet in dto.ChiTietThuocList)
            {
                if (!danhSachThuocHopLe.Any(thuoc => thuoc.MaThuoc == chiTiet.MaThuoc))
                    throw new NotFoundException($"Mã thuốc với ID [{chiTiet.MaThuoc}] không tồn tại.");
            }


            return _donThuocMapping.MapEntityToDto(
                await _donThuocRepository.CreateAsync(
                    _donThuocMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            var donThuoc = await GetByIdAsync(id);
           
            await _ketQuaDieuTriService.DeleteByMaHoSoYTeAsync(donThuoc.MaHoSoYte);

            await _donThuocRepository.DeleteAsync(
                _donThuocMapping.MapDtoToEntity(
                    donThuoc));
        }

        public async Task DeleteByMaHoSoYTeAsync(int id)
        {
            var donthuocs = await _donThuocRepository.GetQueryable()
                                .Where(dt => dt.MaHoSoYte == id)
                                .ToListAsync();
            if (donthuocs.Any())
                await _donThuocRepository.DeleteAsync(donthuocs);
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
