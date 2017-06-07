using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Cameras")]
    public class Camera
    {
        public int Id { get; set; }

        public string MayTinhId { get; set; }

        public int? H { get; set; }

        [StringLength(100)]
        public string IP { get; set; }

        [StringLength(120)]
        public string Password { get; set; }

        public int? Port { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public int? W { get; set; }

        public int? X { get; set; }

        public int? Y { get; set; }
    }
}