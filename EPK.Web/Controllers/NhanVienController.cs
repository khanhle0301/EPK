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
    [RoutePrefix("api/nhanvien")]
    public class NhanVienController : ApiControllerBase
    {
        private readonly INhanVienService _nhanVienService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="nhanVienService"></param>
        public NhanVienController(INhanVienService nhanVienService)
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

                var model = result.Content.ReadAsAsync<IEnumerable<NhanVien>>().Result;

                IEnumerable<NhanVienViewModel> modelVm = Mapper.Map<IEnumerable<NhanVien>, IEnumerable<NhanVienViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);

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
        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _nhanVienService.GetAll();
                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<NhanVien>>().Result;

                int totalRow = model.Count();

                var query = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);

                IEnumerable<NhanVienViewModel> modelVm = Mapper.Map<IEnumerable<NhanVien>, IEnumerable<NhanVienViewModel>>(query);

                PaginationSet<NhanVienViewModel> pagedSet = new PaginationSet<NhanVienViewModel>()
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
                var model = _nhanVienService.GetById(id);

                if (!model.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(model.StatusCode, model.Content.ReadAsStringAsync().Result);
                }

                var query = model.Content.ReadAsAsync<NhanVien>().Result;

                var responseData = Mapper.Map<NhanVien, NhanVienViewModel>(query);

                var response = request.CreateResponse(model.StatusCode, responseData);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, NhanVien nhanVien)
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
                    var result = _nhanVienService.Add(nhanVien);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<NhanVien>().Result;
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

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, NhanVien nhanVien)
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
                    var result = _nhanVienService.Update(nhanVien);
                    if (result.IsSuccessStatusCode)
                    {
                        var newNhanvien = result.Content.ReadAsAsync<NhanVien>().Result;
                        response = request.CreateResponse(result.StatusCode, newNhanvien);
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
                    var result = _nhanVienService.Delete(id);
                    if (result.IsSuccessStatusCode)
                    {
                        var nhanvien = result.Content.ReadAsAsync<NhanVien>().Result;
                        response = request.CreateResponse(result.StatusCode, nhanvien);
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
                    var result = _nhanVienService.DeleteMulti(listId);
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