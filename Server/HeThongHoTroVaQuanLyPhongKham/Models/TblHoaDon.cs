using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblHoaDon
{
    public int MaHoaDon { get; set; }

    public int MaLichHen { get; set; }

    public decimal TongTien { get; set; }

    public DateTime NgayThanhToan { get; set; }

    public string TrangThaiThanhToan { get; set; } = null!;

    public virtual TblLichHen MaLichHenNavigation { get; set; } = null!;
}
