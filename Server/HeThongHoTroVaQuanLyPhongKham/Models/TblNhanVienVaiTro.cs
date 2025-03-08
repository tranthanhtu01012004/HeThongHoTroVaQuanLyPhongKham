using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblNhanVienVaiTro
{
    public int MaNhanVien { get; set; }

    public int MaVaiTro { get; set; }

    public string? CaLamViec { get; set; }

    public string? ChuyenMon { get; set; }

    public virtual TblNhanVien MaNhanVienNavigation { get; set; } = null!;

    public virtual TblVaiTro MaVaiTroNavigation { get; set; } = null!;
}
