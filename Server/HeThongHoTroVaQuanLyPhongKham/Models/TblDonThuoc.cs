using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDonThuoc
{
    public int MaDonThuoc { get; set; }

    public DateTime NgayKeDon { get; set; }

    public virtual ICollection<TblDonThuocChiTiet> TblDonThuocChiTiets { get; set; } = new List<TblDonThuocChiTiet>();

    public virtual ICollection<TblKetQuaDieuTri> TblKetQuaDieuTris { get; set; } = new List<TblKetQuaDieuTri>();
}
