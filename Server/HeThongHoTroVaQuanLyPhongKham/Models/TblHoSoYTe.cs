using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblHoSoYTe
{
    public int MaHoSoYte { get; set; }

    public int MaBenhNhan { get; set; }

    public string? ChuanDoan { get; set; }

    public string? PhuongPhapDieuTri { get; set; }

    public string? LichSuBenh { get; set; }

    public virtual TblBenhNhan MaBenhNhanNavigation { get; set; } = null!;

    public virtual ICollection<TblDonThuoc> TblDonThuocs { get; set; } = new List<TblDonThuoc>();

    public virtual ICollection<TblKetQuaDieuTri> TblKetQuaDieuTris { get; set; } = new List<TblKetQuaDieuTri>();

    public virtual ICollection<TblKetQuaXetNghiem> TblKetQuaXetNghiems { get; set; } = new List<TblKetQuaXetNghiem>();

    public virtual ICollection<TblTrieuChung> TblTrieuChungs { get; set; } = new List<TblTrieuChung>();
}
