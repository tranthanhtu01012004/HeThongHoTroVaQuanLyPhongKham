using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblLichSuThayDoi
{
    public int MaLichSu { get; set; }

    public int MaNhanVien { get; set; }

    public int MaBanGhi { get; set; }

    public DateTime ThoiGian { get; set; }

    public string BangLienQuan { get; set; } = null!;

    public string HanhDong { get; set; } = null!;

    public virtual TblNhanVien MaNhanVienNavigation { get; set; } = null!;
}
