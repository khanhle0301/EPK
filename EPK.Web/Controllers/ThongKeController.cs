using EPK.Data.Common.ViewModels;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EPK.Data.Resources;

namespace EPK.Web.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Authorize]
    [RoutePrefix("api/thongke")]
    public class ThongKeController : ApiControllerBase
    {
        private readonly IThongKeService _thongKeService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="thongKeService"></param>
        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("thongkegiahan")]
        [HttpGet]
        public HttpResponseMessage ThongKeGiaHan(HttpRequestMessage request, int page, int pageSize, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _thongKeService.GetAll(CurrentLink.ThongKeGiaHan + "?fromDate=" + fromDate + "&toDate=" + toDate);

                int totalRow = 0;

                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                PaginationSet<ThongKeGianHanViewModel> pagedSet = new PaginationSet<ThongKeGianHanViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = query
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}