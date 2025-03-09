using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Dtos.HeThongHoTroVaQuanLyPhongKham.DTOs;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class LichHenService : BaseService, ILichHenService
    {
        private readonly IRepository<TblLichHen> _lichHenRepository;
        private readonly IMapper<LichHenDTO, TblLichHen> _lichHenMapping;

        public LichHenService(IRepository<TblLichHen> lichHenRepository, IMapper<LichHenDTO, TblLichHen> lichHenMapping)
        {
            _lichHenRepository = lichHenRepository;
            _lichHenMapping = lichHenMapping;
        }

        public async Task<LichHenDTO> AddAsync(LichHenDTO dto)
        {
            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.CreateAsync(
                    _lichHenMapping.MapDtoToEntity(dto)));
        }

        public async Task DeleteAsync(int id)
        {
            await _lichHenRepository.DeleteAsync(
                _lichHenMapping.MapDtoToEntity(
                    await GetByIdAsync(id)));
        }

        public async Task<IEnumerable<LichHenDTO>> GetAllAsync(
            int page, int pageSize, 
            DateTime? ngayHen = null, 
            int? maNhanVien = null, 
            int? maPhong = null)
        {
            var query = _lichHenRepository.GetQueryable();

            if (ngayHen.HasValue)
                query = query.Where(lh => lh.NgayHen.Date == ngayHen.Value.Date);

            if (maNhanVien.HasValue)
                query = query.Where(lh => lh.MaNhanVien == maNhanVien.Value);

            if (maPhong.HasValue)
                query = query.Where(lh => lh.MaPhongKham == maPhong.Value);

            var pageSkip = CalculatePageSkip(page, pageSize);
            var lichHens = await _lichHenRepository.FindAllWithQueryAsync(query, page, pageSize, pageSkip, "MaLichHen");

            return lichHens.Select(lh => _lichHenMapping.MapEntityToDto(lh));
        }

        public Task<IEnumerable<LichHenDTO>> GetAllAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<LichHenDTO> GetByIdAsync(int id)
        {
            var lichHen = await _lichHenRepository.FindByIdAsync(id, "MaLichHen");
            if (lichHen is null)
                throw new NotFoundException($"Lịch hẹn với ID [{id}] không tồn tại.");

            return _lichHenMapping.MapEntityToDto(lichHen);
        }

        public async Task<LichHenDTO> UpdateAsync(LichHenDTO dto)
        {
            var lichHenUpdate = _lichHenMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaLichHen));

            _lichHenMapping.MapDtoToEntity(dto, lichHenUpdate);

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.UpdateAsync(lichHenUpdate));
        }

        public async Task<LichHenDTO> UpdateTrangThaiAsync(int maLichHen, LichHenUpdateDTO dto)
        {
            var lichHen = await GetByIdAsync(maLichHen);

            lichHen.TrangThai = dto.TrangThai;

            return _lichHenMapping.MapEntityToDto(
                await _lichHenRepository.UpdateAsync(
                _lichHenMapping.MapDtoToEntity(lichHen)));
        }
    }
}
