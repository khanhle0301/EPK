using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("LichSus")]
    public class LichSu
    {
        public int Id { get; set; }

        public DateTime? Ngay { get; set; }

        public int? MayTinhId { get; set; }

        public int? NhanVienId { get; set; }

        public string NoiDung { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}