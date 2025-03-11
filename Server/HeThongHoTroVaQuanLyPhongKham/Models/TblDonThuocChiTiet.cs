using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuocChiTiet
{
    public int MaDonThuocChiTiet { get; set; }

    public int MaDonThuoc { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public string CachDung { get; set; } = null!;

    public string? LieuLuong { get; set; }

    public string? TanSuat { get; set; }

    public virtual TblDonThuoc MaDonThuocNavigation { get; set; } = null!;

    public virtual TblThuoc MaThuocNavigation { get; set; } = null!;
}
