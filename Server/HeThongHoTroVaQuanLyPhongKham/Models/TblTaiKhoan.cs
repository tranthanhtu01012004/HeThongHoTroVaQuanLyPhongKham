using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblTaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public int MaVaiTro { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public virtual TblVaiTro MaVaiTroNavigation { get; set; } = null!;

    public virtual ICollection<TblNhanVien> TblNhanViens { get; set; } = new List<TblNhanVien>();
}
