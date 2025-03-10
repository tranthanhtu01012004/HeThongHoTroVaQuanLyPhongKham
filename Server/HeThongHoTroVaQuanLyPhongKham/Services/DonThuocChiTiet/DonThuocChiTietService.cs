using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;

namespace HeThongHoTroVaQuanLyPhongKham.Services.DonThuocChiTiet
{
    public class DonThuocChiTietService : BaseService, IDonThuocChiTietService
    {
        private readonly IRepository<TblDonThuocChiTiet> _donThuocChiTietRepository;
        private readonly IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> _donThuocChiTietMapping;

        public DonThuocChiTietService(IRepository<TblDonThuocChiTiet> donThuocChiTietRepository, IMapper<DonThuocChiTietDTO, TblDonThuocChiTiet> donThuocChiTietMapping)
        {
            _donThuocChiTietRepository = donThuocChiTietRepository;
            _donThuocChiTietMapping = donThuocChiTietMapping;
        }

        public async Task<DonThuocChiTietDTO> AddAsync(DonThuocChiTietDTO dto)
        {
            return _donThuocChiTietMapping.MapEntityToDto(
                await _donThuocChiTietRepository.CreateAsync(
                    _donThuocChiTietMapping.MapDtoToEntity(dto)));
        }

        public async Task AddAsync(IEnumerable<DonThuocChiTietDTO> dto)
        {
            foreach(var chiTiet in dto)
            {
                await _donThuocChiTietRepository.CreateAsync(
                    _donThuocChiTietMapping.MapDtoToEntity(chiTiet)
                );
            }
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DonThuocChiTietDTO>> GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<DonThuocChiTietDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DonThuocChiTietDTO> UpdateAsync(DonThuocChiTietDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
