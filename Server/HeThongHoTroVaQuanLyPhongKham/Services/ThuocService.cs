using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class ThuocService : BaseService, IService<ThuocDTO>
    {
        private readonly IRepository<TblThuoc> _thuocRepository;
        private readonly IMapper<ThuocDTO, TblThuoc> _thuocMapping;

        public ThuocService(IRepository<TblThuoc> thuocRepository, IMapper<ThuocDTO, TblThuoc> thuocMapping)
        {
            _thuocRepository = thuocRepository;
            _thuocMapping = thuocMapping;
        }

        public async Task<ThuocDTO> AddAsync(ThuocDTO dto)
        {
            return _thuocMapping.MapEntityToDto(
                await _thuocRepository.CreateAsync(
                    _thuocMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _thuocRepository.DeleteAsync(
                _thuocMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<ThuocDTO>> GetAllAsync(int page, int pageSize)
        {
            var pageSkip = CalculatePageSkip(page, pageSize);
            var thuocs = await _thuocRepository.FindAllAsync(page, pageSize, pageSkip, "MaThuoc");
            return thuocs.Select(t => _thuocMapping.MapEntityToDto(t));
        }

        public async Task<ThuocDTO> GetByIdAsync(int id)
        {
            var thuoc = await _thuocRepository.FindByIdAsync(id, "MaThuoc");
            if (thuoc is null)
                throw new NotFoundException($"Thuốc với ID [{id}] không tồn tại.");

            return _thuocMapping.MapEntityToDto(thuoc);
        }

        public async Task<ThuocDTO> UpdateAsync(ThuocDTO dto)
        {
            var thuocUpdate = _thuocMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaThuoc));

            _thuocMapping.MapDtoToEntity(dto, thuocUpdate);

            return _thuocMapping.MapEntityToDto(
                await _thuocRepository.UpdateAsync(thuocUpdate));
        }
    }
}
