using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Ras")]
    public class Ra
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaThe { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        public DateTime? GioVao { get; set; }

        public DateTime? GioRa { get; set; }

        public long? GiaVe { get; set; }

        public int? LoaiVeId { get; set; }

        public int? MayTinhIdVao { get; set; }

        public int? MayTinhIdRa { get; set; }

        public int? NhanVienIdVao { get; set; }

        public int? NhanVienIdRa { get; set; }

        [ForeignKey("LoaiVeId")]
        public virtual LoaiVe LoaiVe { set; get; }
    }
}