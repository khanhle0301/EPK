using System;

namespace EPK.Data.Common.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class ThongKeGianHanViewModel
    {
        public int Id { get; set; }

        public string MaThe { get; set; }

        public string BienSo { get; set; }

        public string HoTen { get; set; }

        public DateTime? NgayGiaHan { get; set; }

        public long? GiaVe { get; set; }

        public DateTime? NgayHetHan { get; set; }

        public string  NhanVien { get; set; }
    }
}