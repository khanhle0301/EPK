using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Api/applicationRole")]
    public class ApplicationRoleController : ApiControllerBase
    {
        private IApplicationRoleService _appRoleService;

        public ApplicationRoleController(
            IApplicationRoleService appRoleService)
        {
            _appRoleService = appRoleService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _appRoleService.GetAll();

                var model = result.Content.ReadAsAsync<IEnumerable<ApplicationRole>>().Result;

                var response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }
    }
}