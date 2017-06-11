using System;

namespace EPK.Web.Models
{
    public class TheViewModel
    {
        public string Id { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgaySua { get; set; }

        public bool? DangSuDung { get; set; }

        public int? BaiXeId { get; set; }
    }
}