using EPK.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Web.Models
{
    public class VeThangViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MaThe { get; set; }

        [StringLength(50)]
        public string BienSo { get; set; }

        public long? GiaVe { get; set; }

        public DateTime? NgayDangKy { get; set; }

        public DateTime? NgayHetHan { get; set; }

        [StringLength(300)]
        public string LoaiXe { get; set; }

        [StringLength(300)]
        public string HoTen { get; set; }

        public string HinhAnh { get; set; }

        public int? DanhMucId { get; set; }

        public string GhiChu { get; set; }

        public bool? XeMay { get; set; }

        public bool? ThuThe { get; set; }

        public bool? DangSuDung { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }

        [ForeignKey("DanhMucId")]
        public virtual DmVeThang DmVeThang { set; get; }
    }
}