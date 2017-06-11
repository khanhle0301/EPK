using AutoMapper;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
using System;
using System.Collections.Generic;
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
                var result = _theService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<The>>().Result;

                int totalRow = model.Count();

                var query = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);

                IEnumerable<TheViewModel> modelVm = Mapper.Map<IEnumerable<The>, IEnumerable<TheViewModel>>(query);

                PaginationSet<TheViewModel> pagedSet = new PaginationSet<TheViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, The the)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var result = _theService.Update(the);
                    if (result.IsSuccessStatusCode)
                    {
                        var newThe = result.Content.ReadAsAsync<The>().Result;
                        response = request.CreateResponse(result.StatusCode, newThe);
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var result = _theService.Delete(id);
                    if (result.IsSuccessStatusCode)
                    {
                        var theOld = result.Content.ReadAsAsync<The>().Result;
                        response = request.CreateResponse(result.StatusCode, theOld);
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }
                return response;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string listId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var result = _theService.DeleteMulti(listId);
                    if (result.IsSuccessStatusCode)
                    {
                        var count = result.Content.ReadAsAsync<int>().Result;
                        response = request.CreateResponse(result.StatusCode, count);
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }

                return response;
            });
        }
    }
}