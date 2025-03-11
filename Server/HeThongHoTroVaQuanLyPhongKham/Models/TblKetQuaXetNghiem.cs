using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblKetQuaXetNghiem
{
    public int MaKetQua { get; set; }

    public int MaHoSoYte { get; set; }

    public string TenXetNghiem { get; set; } = null!;

    public string? KetQua { get; set; }

    public DateTime NgayXetNghiem { get; set; }

    public virtual TblHoSoYTe MaHoSoYteNavigation { get; set; } = null!;
}
