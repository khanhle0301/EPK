using EPK.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Web.Models
{
    public class ApplicationGroupViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? BaiXeId { get; set; }

        [ForeignKey("BaiXeId")]
        public virtual BaiXe BaiXe { set; get; }

        public IEnumerable<ApplicationRole> Roles { set; get; }

        public bool DangSuDung { set; get; }
    }
}