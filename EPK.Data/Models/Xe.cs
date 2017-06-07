using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Xes")]
    public class Xe
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        public int? LoaiXeId { get; set; }

        [ForeignKey("LoaiXeId")]
        public virtual LoaiXe LoaiXe { set; get; }
    }
}