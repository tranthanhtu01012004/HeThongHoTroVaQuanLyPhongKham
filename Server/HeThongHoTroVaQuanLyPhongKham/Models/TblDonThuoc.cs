using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuoc
{
    public int MaDonThuoc { get; set; }

    public int MaHoSoYte { get; set; }

    public DateTime NgayKeDon { get; set; }

    public virtual TblHoSoYTe MaHoSoYteNavigation { get; set; } = null!;

    public virtual ICollection<TblDonThuocChiTiet> TblDonThuocChiTiets { get; set; } = new List<TblDonThuocChiTiet>();
}
