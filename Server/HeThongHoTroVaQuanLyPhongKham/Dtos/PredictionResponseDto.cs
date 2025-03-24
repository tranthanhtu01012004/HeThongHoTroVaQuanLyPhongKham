using System.Text.Json.Serialization;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class PredictionResponseDto
    {
        [JsonPropertyName("diagnosis")]
        public string Diagnosis { get; set; }

        [JsonPropertyName("treatment")]
        public string Treatment { get; set; }

        [JsonPropertyName("medicines")]
        public List<MedicineDto> Medicines { get; set; }

        [JsonPropertyName("warning")]
        public string Warning { get; set; }
    }
}
