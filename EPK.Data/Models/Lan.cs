using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Lans")]
    public class Lan
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Ten { get; set; }

        [StringLength(50)]
        public string Led { get; set; }

        public byte? Port { get; set; }

        public bool? LanType { get; set; }
    }
}