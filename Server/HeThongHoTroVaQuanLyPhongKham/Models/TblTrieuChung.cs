using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblTrieuChung
{
    public int MaTrieuChung { get; set; }

    public int MaHoSoYte { get; set; }

    public string TenTrieuChung { get; set; } = null!;

    public string? MoTa { get; set; }

    public DateTime? ThoiGianXuatHien { get; set; }

    public virtual TblHoSoYTe MaHoSoYteNavigation { get; set; } = null!;
}
