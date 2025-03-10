using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuocChiTiet
{
    public int MaDonThuoc { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public string CachDung { get; set; } = null!;

    public virtual TblDonThuoc MaDonThuocNavigation { get; set; } = null!;

    public virtual TblThuoc MaThuocNavigation { get; set; } = null!;
}
