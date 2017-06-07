using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("KhungGios")]
    public class KhungGio
    {
        public int Id { get; set; }

        public long? Gia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GioBatDau { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GioKetThuc { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}