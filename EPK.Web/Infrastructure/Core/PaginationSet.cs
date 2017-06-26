using EPK.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPK.Web.Infrastructure.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationSet<T>
    {
        /// <summary>
        ///
        /// </summary>
        public int Page { set; get; }

        /// <summary>
        ///
        /// </summary>
        public int Count => Items?.Count() ?? 0;

        /// <summary>
        ///
        /// </summary>
        public int TotalPages { set; get; }

        /// <summary>
        ///
        /// </summary>
        public int TotalCount { set; get; }

        /// <summary>
        ///
        /// </summary>
        public int MaxPage { set; get; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<T> Items { set; get; }

        //public int SoLuotXe { set; get; }

        //public int XeVangLai { set; get; }

        //public List<ListXe> ListXeVangLai { set; get; }

        //public int XeThang { set; get; }

        public SumThongKeChiTiet SumThongKeChiTiet { set; get; }
    }
}