using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("BaiXes")]
    public class BaiXe
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string TenBai { get; set; }

        public int? CongTyId { get; set; }

        [ForeignKey("CongTyId")]
        public virtual CongTy CongTy { set; get; }
    }
}