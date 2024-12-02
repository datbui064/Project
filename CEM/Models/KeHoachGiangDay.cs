using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class KeHoachGiangDay
{
    public int MaKeHoach { get; set; }

    public string? TieuDe { get; set; }

    public string? LoaiKeHoach { get; set; }

    public string? ChiTiet { get; set; }

    public int? MaBaiGiang { get; set; }

    public int? MaNguoiDung { get; set; }

    public virtual BaiGiang? MaBaiGiangNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
