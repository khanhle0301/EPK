using EPK.Data.Models;
using EPK.Data.Resources;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPK.Web.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Authorize]
    [RoutePrefix("api/the")]
    public class TheController : ApiControllerBase
    {
        private readonly ITheService _theService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="theService"></param>
        public TheController(ITheService theService)
        {
            _theService = theService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _theService.GetAll(CurrentLink.GetAllThe);

                int totalRow = 0;

                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                PaginationSet<The> pagedSet = new PaginationSet<The>()
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