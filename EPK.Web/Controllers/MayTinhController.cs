using AutoMapper;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/maytinh")]
    public class MayTinhController : ApiControllerBase
    {
        private readonly IMayTinhService _nhanVienService;

        public MayTinhController(IMayTinhService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _nhanVienService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<MayTinh>>().Result;

                IEnumerable<MayTinhViewModel> modelVm = Mapper.Map<IEnumerable<MayTinh>, IEnumerable<MayTinhViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }
    }
}