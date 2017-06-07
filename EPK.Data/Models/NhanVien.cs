using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("NhanViens")]
    public class NhanVien
    {
        public int Id { get; set; }

        public int? DanhMucId { get; set; }

        [StringLength(300)]
        public string Ten { get; set; }

        [StringLength(300)]
        public string DiaChi { get; set; }

        [StringLength(300)]
        public string SoDienThoai { get; set; }

        public long? Luong { get; set; }

        public string GhiChu { get; set; }

        public bool? DangSuDung { get; set; }

        [ForeignKey("DanhMucId")]
        public virtual DmNhanVien DmNhanVien { set; get; }
    }
}