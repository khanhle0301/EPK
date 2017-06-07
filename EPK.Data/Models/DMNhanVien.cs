using System.ComponentModel.DataAnnotations;

namespace EPK.Data.Models
{
    public class DmNhanVien
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Ten { get; set; }

        [StringLength(300)]
        public string GhiChu { get; set; }

        public bool? DangSuDung { get; set; }

        public int? BaiXeId { get; set; }
    }
}