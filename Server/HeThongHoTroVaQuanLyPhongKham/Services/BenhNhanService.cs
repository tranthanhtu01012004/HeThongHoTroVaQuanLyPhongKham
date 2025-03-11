using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class BenhNhanService : BaseService, IService<BenhNhanDTO>
    {
        private readonly IRepository<TblBenhNhan> _benhNhanRepository;
        private readonly IMapper<BenhNhanDTO, TblBenhNhan> _benhNhanMapping;

        public BenhNhanService(IRepository<TblBenhNhan> benhNhanRepository, IMapper<BenhNhanDTO, TblBenhNhan> benhNhanMapping)
        {
            _benhNhanRepository = benhNhanRepository;
            _benhNhanMapping = benhNhanMapping;
        }

        public async Task<BenhNhanDTO> AddAsync(BenhNhanDTO dto)
        {
            return _benhNhanMapping.MapEntityToDto(
                await _benhNhanRepository.CreateAsync(
                    _benhNhanMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _benhNhanRepository.DeleteAsync(
                _benhNhanMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<BenhNhanDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var benhNhans = await _benhNhanRepository.FindAllAsync(page, pageSize, pageSkip, "MaBenhNhan");
            return benhNhans.Select(t => _benhNhanMapping.MapEntityToDto(t));
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
