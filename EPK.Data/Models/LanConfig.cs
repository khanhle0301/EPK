using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("LanConfigs")]
    public class LanConfig
    {
        [Key]
        [Column(Order = 1)]
        public int LanId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int CameraId { set; get; }

        [Key]
        [Column(Order=3)]
        public string MayTinhId { set; get; }

        [ForeignKey("LanId")]
        public virtual Lan Lan { set; get; }

        [ForeignKey("CameraId")]
        public virtual Camera Camera { set; get; }

        [ForeignKey("MayTinhId")]
        public virtual MayTinh MayTinh { set; get; }
    }
}