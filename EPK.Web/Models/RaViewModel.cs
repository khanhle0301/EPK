using System;

namespace EPK.Web.Models
{
    public class RaViewModel
    {
        public string MaThe { get; set; }

        public string BienSo { get; set; }

        public DateTime? GioVao { get; set; }

        public DateTime GioRa { get; set; }

        public long? GiaVe { get; set; }

        public int? LoaiVeId { get; set; }

        public string MayTinhId_Vao { get; set; }

        public string MayTinhId_Ra { get; set; }

        public string NhanVienId_Vao { get; set; }

        public string NhanVienId_Ra { get; set; }

        public int? BaiXeID { get; set; }
    }
}