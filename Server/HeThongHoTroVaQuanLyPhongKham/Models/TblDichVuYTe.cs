using System;
using System.Collections.Generic;

namespace HeThongHoTroVaQuanLyPhongKham.Models;

public partial class TblDichVuYTe
{
    public int MaDichVuYte { get; set; }

    public string Ten { get; set; } = null!;

    public decimal ChiPhi { get; set; }

    public virtual ICollection<TblLichHen> TblLichHens { get; set; } = new List<TblLichHen>();
}
