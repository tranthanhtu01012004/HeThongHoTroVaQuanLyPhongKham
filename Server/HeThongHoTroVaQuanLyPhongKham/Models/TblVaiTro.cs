using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblVaiTro
{
    public int MaVaiTro { get; set; }

    public string Ten { get; set; } = null!;

    public virtual ICollection<TblNhanVienVaiTro> TblNhanVienVaiTros { get; set; } = new List<TblNhanVienVaiTro>();
}
