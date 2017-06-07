using System;
using System.ComponentModel.DataAnnotations;

namespace EPK.Web.Models
{
    public class GiaHanViewModel
    {
        public int Id { get; set; }

        public int? VeThangId { get; set; }

        [StringLength(50)]
        public string MaThe { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        [StringLength(300)]
        public string HoTen { get; set; }

        public DateTime? NgayGiaHan { get; set; }

        public long? GiaVe { get; set; }

        public int? MayTinhId { get; set; }

        public int? NhanVienId { get; set; }
    }
}