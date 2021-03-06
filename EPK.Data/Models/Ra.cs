using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Ras")]
    public class Ra
    {
        [StringLength(50)]
        public string MaThe { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        public DateTime? GioVao { get; set; }

        public DateTime GioRa { get; set; }

        public long? GiaVe { get; set; }

        public int? LoaiVeId { get; set; }

        public string MayTinhId_Vao { get; set; }

        public string MayTinhId_Ra { get; set; }

        public string NhanVienId_Vao { get; set; }

        public string NhanVienId_Ra { get; set; }

        public int? BaiXeID { get; set; }
    }
}