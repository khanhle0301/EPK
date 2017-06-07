using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("CongTys")]
    public class CongTy
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string Ten { get; set; }
    }
}