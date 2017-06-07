using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("LoaiXes")]
    public class LoaiXe
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Ten { get; set; }

        public bool? ThuTienTruoc { get; set; }

        public bool? DangSuDung { get; set; }

        public int? LoaiVeId { get; set; }

        [ForeignKey("LoaiVeId")]
        public virtual LoaiVe LoaiVe { set; get; }
    }
}