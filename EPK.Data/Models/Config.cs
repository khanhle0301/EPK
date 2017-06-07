using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPK.Data.Models
{
    [Table("Configs")]
    public class Config
    {
        public int Id { get; set; }

        public bool? Audio { get; set; }

        public int? AutoSave { get; set; }

        public bool? AutoSave_OnOff { get; set; }

        public bool? AutoSave_UI { get; set; }

        [StringLength(50)]
        public string CardReaderType { get; set; }

        public bool? DocBienSo { get; set; }

        public bool? DocBienSo_UI { get; set; }

        [StringLength(500)]
        public string DuPhong { get; set; }

        [StringLength(500)]
        public string FolderBackup_Db { get; set; }

        [StringLength(500)]
        public string ImagesFolder { get; set; }

        [StringLength(500)]
        public string ImagesFolderTemp { get; set; }

        [StringLength(300)]
        public string IP { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(50)]
        public string Led { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PortBarrier { get; set; }

        public int? SpeedReader { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public string MayTinhId { get; set; }

        [ForeignKey("MayTinhId")]
        public virtual MayTinh MayTinh { set; get; }
    }
}