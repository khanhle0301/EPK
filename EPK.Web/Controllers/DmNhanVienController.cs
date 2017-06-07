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
    [RoutePrefix("api/dmnhanvien")]
    public class DmNhanVienController : ApiControllerBase
    {
        private readonly IDmNhanVienService _dmNhanVienService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmNhanVienService"></param>
        public DmNhanVienController(IDmNhanVienService dmNhanVienService)
        {
            _dmNhanVienService = dmNhanVienService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _dmNhanVienService.GetById(id);

                if (!model.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(model.StatusCode, model.ReasonPhrase);
                }

                var query = model.Content.ReadAsAsync<DmNhanVien>().Result;

                var responseData = Mapper.Map<DmNhanVien, DmNhanVienViewModel>(query);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
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
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _dmNhanVienService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<DmNhanVien>>().Result;

                int totalRow = model.Count();

                var query = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);

                IEnumerable<DmNhanVienViewModel> modelVm = Mapper.Map<IEnumerable<DmNhanVien>, IEnumerable<DmNhanVienViewModel>>(query);

                PaginationSet<DmNhanVienViewModel> pagedSet = new PaginationSet<DmNhanVienViewModel>()
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

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _dmNhanVienService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<DmNhanVien>>().Result;

                IEnumerable<DmNhanVienViewModel> modelVm = Mapper.Map<IEnumerable<DmNhanVien>, IEnumerable<DmNhanVienViewModel>>(model);

                var response = request.CreateResponse(result.StatusCode, modelVm);

                return response;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="dmNhanVien"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, DmNhanVien dmNhanVien)
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
                    var result = _dmNhanVienService.Add(dmNhanVien);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<DmNhanVien>().Result;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="dmNhanVien"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, DmNhanVien dmNhanVien)
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
                    var result = _dmNhanVienService.Update(dmNhanVien);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<DmNhanVien>().Result;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    var result = _dmNhanVienService.Delete(id);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<DmNhanVien>().Result;
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
                    var result = _dmNhanVienService.DeleteMulti(listId);
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