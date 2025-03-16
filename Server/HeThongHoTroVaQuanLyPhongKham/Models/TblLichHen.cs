using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblLichHen
{
    public int MaLichHen { get; set; }

    public int MaBenhNhan { get; set; }

    public int? MaNhanVien { get; set; }

    public int MaDichVuYte { get; set; }

    public int? MaPhongKham { get; set; }

    public DateTime NgayHen { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual TblBenhNhan MaBenhNhanNavigation { get; set; } = null!;

    public virtual TblDichVuYTe MaDichVuYteNavigation { get; set; } = null!;

    public virtual TblNhanVien? MaNhanVienNavigation { get; set; }

    public virtual TblPhongKham? MaPhongKhamNavigation { get; set; }

    public virtual ICollection<TblHoaDon> TblHoaDons { get; set; } = new List<TblHoaDon>();
}
