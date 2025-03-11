using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblKetQuaDieuTri
{
    public int MaKetQuaDieuTri { get; set; }

    public int MaHoSoYte { get; set; }

    public int MaDonThuoc { get; set; }

    public string? HieuQua { get; set; }

    public string? TacDungPhu { get; set; }

    public DateTime NgayDanhGia { get; set; }

    public virtual TblDonThuoc MaDonThuocNavigation { get; set; } = null!;

    public virtual TblHoSoYTe MaHoSoYteNavigation { get; set; } = null!;
}
