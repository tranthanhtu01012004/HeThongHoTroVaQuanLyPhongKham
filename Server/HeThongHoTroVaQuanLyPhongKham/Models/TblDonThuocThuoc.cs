using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuocThuoc
{
    public int MaDonThuoc { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public virtual TblDonThuoc MaDonThuocNavigation { get; set; } = null!;

    public virtual TblThuoc MaThuocNavigation { get; set; } = null!;
}
