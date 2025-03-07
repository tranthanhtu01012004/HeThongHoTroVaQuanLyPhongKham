using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuoc
{
    public int MaDonThuoc { get; set; }

    public int MaHoSoYte { get; set; }

    public string LieuLuong { get; set; } = null!;

    public virtual TblHoSoYTe MaHoSoYteNavigation { get; set; } = null!;

    public virtual ICollection<TblDonThuocThuoc> TblDonThuocThuocs { get; set; } = new List<TblDonThuocThuoc>();
}
