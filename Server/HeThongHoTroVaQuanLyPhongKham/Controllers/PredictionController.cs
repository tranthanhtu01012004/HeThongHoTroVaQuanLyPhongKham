using HeThongHoTroVaQuanLyPhongKham.Dtos;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using HeThongHoTroVaQuanLyPhongKham.Services;
using HeThongHoTroVaQuanLyPhongKham.Common;
using Microsoft.AspNetCore.Authorization;

namespace HeThongHoTroVaQuanLyPhongKham.Controllers
{
    [Route("api/admin/prediction")]
    [ApiController]
    [AllowAnonymous]
    public class PredictionController : ControllerBase
    {
        private readonly IMLService _mlService;

        public PredictionController(IMLService mlService)
        {
            _mlService = mlService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromBody] PredictionRequestDto request)
        {
            try
            {
                var prediction = await _mlService.Predict(request);
                return Ok(ApiResponse<PredictionResponseDto>.Success(
                    prediction, "Dự đoán chẩn đoán, phương pháp điều trị và thuốc thành công."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message));
            }
            catch (JsonException ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message));
            }
        }
    }
}
