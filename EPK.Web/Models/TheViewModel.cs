using System;
using System.ComponentModel.DataAnnotations;

namespace EPK.Web.Models
{
    public class TheViewModel
    {
        [StringLength(30)]
        public string Id { get; set; }

        public int? BaiXeId { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        public bool? DangSuDung { get; set; }
    }
}