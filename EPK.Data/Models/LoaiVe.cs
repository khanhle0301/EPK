using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("LoaiVes")]
    public class LoaiVe
    {
        public int Id { get; set; }

        public int? KhungGioId { get; set; }

        [StringLength(300)]
        public string Ten { get; set; }

        public bool? CongDon { get; set; }

        public bool? DangSuDung { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}