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
    [Authorize]
    [RoutePrefix("api/dmvethang")]
    public class DmVeThangController : ApiControllerBase
    {
        private readonly IDmVeThangService _dmVeThangService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmVeThangService"></param>
        public DmVeThangController(IDmVeThangService dmVeThangService)
        {
            _dmVeThangService = dmVeThangService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _dmVeThangService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<DmVeThang>>().Result;

                int totalRow = model.Count();

                var query = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);

                IEnumerable<DmVeThangViewModel> modelVm = Mapper.Map<IEnumerable<DmVeThang>, IEnumerable<DmVeThangViewModel>>(query);

                PaginationSet<DmVeThangViewModel> pagedSet = new PaginationSet<DmVeThangViewModel>()
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

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _dmVeThangService.GetById(id);

                if (!model.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(model.StatusCode, model.ReasonPhrase);
                }

                var query = model.Content.ReadAsAsync<DmVeThang>().Result;

                var responseData = Mapper.Map<DmVeThang, DmVeThangViewModel>(query);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _dmVeThangService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<DmVeThang>>().Result;

                IEnumerable<DmVeThangViewModel> modelVm = Mapper.Map<IEnumerable<DmVeThang>, IEnumerable<DmVeThangViewModel>>(model);

                var response = request.CreateResponse(result.StatusCode, modelVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, DmVeThang dmVeThang)
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
                    var result = _dmVeThangService.Add(dmVeThang);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmVeThang = result.Content.ReadAsAsync<DmVeThang>().Result;
                        response = request.CreateResponse(result.StatusCode, newDmVeThang);
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, DmVeThang dmVeThang)
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
                    var result = _dmVeThangService.Update(dmVeThang);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<DmVeThang>().Result;
                        response = request.CreateResponse(result.StatusCode, newDmNhanvien);
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
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    var result = _dmVeThangService.Delete(id);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<DmVeThang>().Result;
                        response = request.CreateResponse(result.StatusCode, newDmNhanvien);
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }
                return response;
            });
        }

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
                    var result = _dmVeThangService.DeleteMulti(listId);
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