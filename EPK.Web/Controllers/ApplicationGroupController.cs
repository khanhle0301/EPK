using AutoMapper;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Infrastructure.Extensions;
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
    [RoutePrefix("Api/applicationGroup")]
    public class ApplicationGroupController : ApiControllerBase
    {
        private readonly IApplicationGroupService _appGroupService;

        public ApplicationGroupController(IApplicationGroupService appGroup)
        {
            _appGroupService = appGroup;
        }

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
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _appGroupService.GetAll();

                var model = result.Content.ReadAsAsync<IEnumerable<ApplicationGroup>>().Result;

                var modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                int totalRow = modelVm.Count();
                PaginationSet<ApplicationGroupViewModel> pagedSet = new PaginationSet<ApplicationGroupViewModel>()
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


        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _appGroupService.GetAll();

                var model = result.Content.ReadAsAsync<IEnumerable<ApplicationGroupViewModel>>().Result;

                //var modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }

            var result = _appGroupService.GetById(id);
            if (!result.IsSuccessStatusCode)
            {
                return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
            }

            var appGroup = result.Content.ReadAsAsync<ApplicationGroupViewModel>().Result;
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, ApplicationGroupViewModel appGroup)
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
                    var newAppGroup = new ApplicationGroup();
                    newAppGroup.UpdateApplicationGroup(appGroup);
                    var result = _appGroupService.Add(newAppGroup);
                    if (result.IsSuccessStatusCode)
                    {
                        var newGroup = result.Content.ReadAsAsync<ApplicationGroupViewModel>().Result;
                        response = request.CreateResponse(result.StatusCode, newGroup);
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
        public HttpResponseMessage Put(HttpRequestMessage request, ApplicationGroup appGroup)
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
                    var result = _appGroupService.Update(appGroup);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<ApplicationGroupViewModel>().Result;
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
                    var result = _appGroupService.Delete(id);
                    if (result.IsSuccessStatusCode)
                    {
                        var appGroup = result.Content.ReadAsAsync<int>().Result;
                        response = request.CreateResponse(result.StatusCode, appGroup);
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
                    var result = _appGroupService.DeleteMulti(listId);
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