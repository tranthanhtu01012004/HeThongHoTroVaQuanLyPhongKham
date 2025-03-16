using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblNhanVien
{
    public int MaNhanVien { get; set; }

    public int MaTaiKhoan { get; set; }

    public string Ten { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? CaLamViec { get; set; }

    public string ChuyenMon { get; set; } = null!;

    public virtual TblTaiKhoan MaTaiKhoanNavigation { get; set; } = null!;

    public virtual ICollection<TblLichHen> TblLichHens { get; set; } = new List<TblLichHen>();

    public virtual ICollection<TblLichSuThayDoi> TblLichSuThayDois { get; set; } = new List<TblLichSuThayDoi>();

    public virtual ICollection<TblPhongKhamNhanVien> TblPhongKhamNhanViens { get; set; } = new List<TblPhongKhamNhanVien>();
}
