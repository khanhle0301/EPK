using EPK.Data.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Web.Models
{
    public class MayTinhViewModel
    {
        public string Id { get; set; }

        public DateTime? NgayTao { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }
    }
}