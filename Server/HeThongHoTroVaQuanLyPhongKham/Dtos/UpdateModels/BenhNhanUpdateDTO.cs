using System.ComponentModel.DataAnnotations;

namespace HeThongHoTroVaQuanLyPhongKham.Dtos.UpdateModels
{
    public class BenhNhanUpdateDTO
    {
        public int MaBenhNhan { get; set; }

        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string? Ten { get; set; }
    }
}
