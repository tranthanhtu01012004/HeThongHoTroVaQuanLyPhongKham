using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblPhongKham
{
    public int MaPhongKham { get; set; }

    public string Loai { get; set; } = null!;

    public int SucChua { get; set; }

    public virtual ICollection<TblLichHen> TblLichHens { get; set; } = new List<TblLichHen>();

    public virtual ICollection<TblPhongKhamNhanVien> TblPhongKhamNhanViens { get; set; } = new List<TblPhongKhamNhanVien>();
}
