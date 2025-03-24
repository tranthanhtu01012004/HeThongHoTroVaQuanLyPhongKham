using System.Text.Json.Serialization;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class MedicineDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("dose")]
        public string Dose { get; set; }

        [JsonPropertyName("frequency")]
        public string Frequency { get; set; }

        [JsonPropertyName("instruction")]
        public string Instruction { get; set; }
    }
}
