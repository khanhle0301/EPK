using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("GiaHans")]
    public class GiaHan
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

        public string MayTinhId { get; set; }

        public string NhanVienId { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}