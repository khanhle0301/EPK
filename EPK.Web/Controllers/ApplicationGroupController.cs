using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace EPK.Web.Controllers
{
    /// <summary>
    ///
    /// </summary>
    /// [Authorize]
    [RoutePrefix("Api/applicationGroup")]
    public class ApplicationGroupController : ApiControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);

                var model = client.GetAsync("/api/applicationGroup/getall").Result.Content.ReadAsAsync<IEnumerable<ApplicationGroup>>().Result;

                int totalRow = 0;
                PaginationSet<ApplicationGroup> pagedSet = new PaginationSet<ApplicationGroup>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = model
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}