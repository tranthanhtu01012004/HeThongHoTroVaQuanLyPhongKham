using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Mappers;
using HeThongHoTroVaQuanLyPhongKham.Models;
using HeThongHoTroVaQuanLyPhongKham.Repositories;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using Microsoft.EntityFrameworkCore;
using HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels;
namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class NhanVienService : BaseService, INhanVienService
    {
        private readonly IRepository<TblNhanVien> _nhanVienRepository;
        private readonly IMapper<NhanVienDTO, TblNhanVien> _nhanVienMapping;
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly IRepository<TblTaiKhoan> _taiKhoanRepository;

        public NhanVienService(IRepository<TblNhanVien> nhanVienRepository, IMapper<NhanVienDTO, TblNhanVien> nhanVienMapping, ITaiKhoanService taiKhoanService, IRepository<TblTaiKhoan> taiKhoanRepository)
        {
            _nhanVienRepository = nhanVienRepository;
            _nhanVienMapping = nhanVienMapping;
            _taiKhoanService = taiKhoanService;
            _taiKhoanRepository = taiKhoanRepository;
        }

        public async Task<NhanVienDTO> AddAsync(NhanVienDTO dto)
        {
            var taiKhoanDTO = new TaiKhoanDTO
            {
                TenDangNhap = dto.TenDangNhap,
                MatKhau = dto.MatKhau,
                MaVaiTro = dto.MaVaiTro
            };

            var taiKhoan = await _taiKhoanService.AddNhanVienAsync(taiKhoanDTO);
            dto.MaTaiKhoan = taiKhoan.MaTaiKhoan;

            return _nhanVienMapping.MapEntityToDto(
                await _nhanVienRepository.CreateAsync(
                    _nhanVienMapping.MapDtoToEntity(dto)));
        }

        public Task<NhanVienDTO> CreateNhanVienWithAccountAsync(NhanVienDTO account)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var taiKhoan = await _taiKhoanRepository.GetQueryable()
                .Include(tk => tk.TblNhanVien)
                .FirstOrDefaultAsync(tk => tk.TblNhanVien.MaNhanVien == id);

            if (taiKhoan is null)
                throw new NotFoundException("Tài khoản không tồn tại.");

            await _taiKhoanRepository.DeleteAsync(taiKhoan);

            // da su dung Cascade
            //await _nhanVienRepository.DeleteAsync(
            //    _nhanVienMapping.MapDtoToEntity(
            //        await GetByIdAsync(id)));
        }

        public async Task<(IEnumerable<NhanVienDTO> Items, int TotalItems, int TotalPages)> GetAllAsync(int page, int pageSize)
        {
            var totalItems = await _nhanVienRepository.CountAsync();
            var totalPages = CalculateTotalPages(totalItems, pageSize);
            var pageSkip = CalculatePageSkip(page, pageSize);
            var nhanViens = await _nhanVienRepository.FindAllAsync(page, pageSize, pageSkip, "MaNhanVien");
            var dtoList = nhanViens.Select(t => _nhanVienMapping.MapEntityToDto(t));
            return (dtoList, totalItems, totalPages);
        }

        public async Task<IEnumerable<NhanVienDTO>> GetAllAsync()
        {
            var nhanViens = await _nhanVienRepository.FindAllAsync("MaNhanVien");
            return nhanViens.Select(t => _nhanVienMapping.MapEntityToDto(t));
        }

        public async Task<NhanVienDTO> GetByIdAsync(int id)
        {
            var taiKhoan = await _nhanVienRepository.FindByIdAsync(id, "MaNhanVien");
            if (taiKhoan is null)
                throw new NotFoundException($"Nhân viên với ID [{id}] không tồn tại.");

            return _nhanVienMapping.MapEntityToDto(taiKhoan);
        }

        public async Task<NhanVienDTO> UpdateAsync(NhanVienDTO dto)
        {
            var nhanVienUpdate = _nhanVienMapping.MapDtoToEntity(
                await GetByIdAsync(dto.MaNhanVien));
            var taiKhoanDTO = new TaiKhoanUpdateDTO
            {
                MaTaiKhoan = (int)dto.MaTaiKhoan,
                MaVaiTro = dto.MaVaiTro
            };

            var taiKhoan = await _taiKhoanService.UpdateAsync(taiKhoanDTO);

            _nhanVienMapping.MapDtoToEntity(dto, nhanVienUpdate);

            return _nhanVienMapping.MapEntityToDto(
                await _nhanVienRepository.UpdateAsync(nhanVienUpdate));
        }
    }
}
