using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("HinhAnhRas")]
    public class HinhAnhRa
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaHinhAnh { get; set; }

        [Column(TypeName = "text")]
        public string HinhAnh { get; set; }

        public int? CamNumber { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}