using HeThongHoTroVaQuanLyPhongKham.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public interface IMLService
    {
        Task<PredictionResponseDto> Predict(PredictionRequestDto request);
    }
}
