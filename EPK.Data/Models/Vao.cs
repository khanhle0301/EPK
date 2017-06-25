using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Vaos")]
    public class Vao
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaThe { get; set; }

        public DateTime? GioVao { get; set; }

        public int? LoaiVeId { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        public string MayTinhId { get; set; }

        public string NhanVienId { get; set; }

        public int? BaiXeId { get; set; }
    }
}