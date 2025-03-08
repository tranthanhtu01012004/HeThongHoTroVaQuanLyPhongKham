using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblHoSoYTe
{
    public int MaHoSoYte { get; set; }

    public int MaBenhNhan { get; set; }

    public string ChuanDoan { get; set; } = null!;

    public string PhuongPhapDieuTri { get; set; } = null!;

    public virtual TblBenhNhan MaBenhNhanNavigation { get; set; } = null!;

    public virtual ICollection<TblDonThuoc> TblDonThuocs { get; set; } = new List<TblDonThuoc>();
}
