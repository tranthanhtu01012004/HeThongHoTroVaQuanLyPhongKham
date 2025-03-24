using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblPhongKhamNhanVien
{
    public int MaPhongKhamNhanVien { get; set; }

    public int MaPhongKham { get; set; }

    public int MaNhanVien { get; set; }

    public string VaiTro { get; set; } = null!;

    public virtual TblNhanVien MaNhanVienNavigation { get; set; } = null!;

    public virtual TblPhongKham MaPhongKhamNavigation { get; set; } = null!;
}
