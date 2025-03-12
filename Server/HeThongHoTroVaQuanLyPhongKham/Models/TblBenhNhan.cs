using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblBenhNhan
{
    public int MaBenhNhan { get; set; }

    public int? Tuoi { get; set; }

    public bool GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public virtual ICollection<TblHoSoYTe> TblHoSoYTes { get; set; } = new List<TblHoSoYTe>();

    public virtual ICollection<TblLichHen> TblLichHens { get; set; } = new List<TblLichHen>();
}
