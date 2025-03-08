using HeThongHoTroVaQuanLyPhongKham.Common;
using HeThongHoTroVaQuanLyPhongKham.Dtos;
using HeThongHoTroVaQuanLyPhongKham.Exceptions;
using HeThongHoTroVaQuanLyPhongKham.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/accounts")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class TaiKhoanController : ControllerBase
    {
        private readonly IService<TaiKhoanDTO> _taiKhoanService;

        public TaiKhoanController(IService<TaiKhoanDTO> taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(ApiResponse<IEnumerable<TaiKhoanDTO>>.Success(
                    await _taiKhoanService.GetAllAsync(page, pageSize)));
            } catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            } catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(ApiResponse<TaiKhoanDTO>.Success(
                    await _taiKhoanService.GetByIdAsync(id), $"Tìm thấy tài khoản với ID [{id}]."));
            } catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            } catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TaiKhoanDTO taiKhoanDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != taiKhoanDTO.MaTaiKhoan)
                    return BadRequest("Id không khớp");

                return Ok(ApiResponse<TaiKhoanDTO>.Success(
                    await _taiKhoanService.UpdateAsync(taiKhoanDTO), $"Cập nhật tài khoản với ID [{id}] thành công."));
            } catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            } catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _taiKhoanService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaiKhoanDTO>.Fail(ex.Message));
            }
        }
    }
}
