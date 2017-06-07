using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("DmVeThangs")]
    public class DmVeThang
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Ten { get; set; }

        [StringLength(300)]
        public string GhiChu { get; set; }

        public bool? DangSuDung { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}