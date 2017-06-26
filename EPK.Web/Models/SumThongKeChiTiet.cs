using System.Collections.Generic;

namespace EPK.Web.Models
{
    public class SumThongKeChiTiet
    {
        public int SoLuotXe { set; get; }

        public int XeVangLai { set; get; }

        public List<ListXe> ListXeVangLai { set; get; }

        public int XeThang { set; get; }
    }
}