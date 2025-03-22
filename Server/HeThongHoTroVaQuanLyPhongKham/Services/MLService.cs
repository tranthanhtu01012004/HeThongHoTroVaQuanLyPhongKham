using System.Text.Json;
using System.Text;
using HeThongHoTroVaQuanLyPhongKham.Dtos;

namespace HeThongHoTroVaQuanLyPhongKham.Services
{
    public class MLService : IMLService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string FlaskApiUrl = "http://localhost:5000/api/prediction/predict";

        public MLService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PredictionResponseDto> Predict(PredictionRequestDto request)
        {
            if (request == null || request.Symptoms == null || !request.Symptoms.Any())
            {
                throw new ArgumentException("Danh sách triệu chứng không được để trống.");
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                // Sửa dữ liệu thành {"symptoms": [...]}
                var requestData = new { symptoms = request.Symptoms };
                var jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(FlaskApiUrl, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var prediction = JsonSerializer.Deserialize<PredictionResponseDto>(jsonResponse);

                if (prediction == null)
                {
                    throw new InvalidOperationException("Không nhận được dữ liệu dự đoán từ Flask API.");
                }

                return prediction;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Lỗi khi gọi Flask API: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Lỗi khi xử lý dữ liệu JSON: {ex.Message}");
            }
        }
    }
}
