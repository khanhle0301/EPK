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
    [RoutePrefix("Api/applicationUser")]
    public class ApplicationUserController : ApiControllerBase
    {
        private readonly IApplicationUserService _appUser;

        /// <summary>
        ///
        /// </summary>
        /// <param name="appUser"></param>
        public ApplicationUserController(IApplicationUserService appUser)
        {
            _appUser = appUser;
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
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _appUser.GetAll();

                var model = result.Content.ReadAsAsync<IEnumerable<ApplicationUser>>().Result;

                var modelVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

                int totalRow = modelVm.Count();
                PaginationSet<ApplicationUserViewModel> pagedSet = new PaginationSet<ApplicationUserViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
            return null;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var result = _appUser.GetAll();

                var model = result.Content.ReadAsAsync<IEnumerable<ApplicationUser>>().Result;

                var modelVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            var result = _appUser.GetById(id);
            if (!result.IsSuccessStatusCode)
            {
                return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
            }

            var appGroup = result.Content.ReadAsAsync<ApplicationUserViewModel>().Result;

            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, ApplicationUserViewModel appGroup)
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
                    var newAppGroup = new ApplicationUser();
                    newAppGroup.UpdateUser(appGroup);
                    var result = _appUser.Add(newAppGroup);
                    if (result.IsSuccessStatusCode)
                    {
                        var newGroup = result.Content.ReadAsAsync<ApplicationUserViewModel>().Result;
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
        public HttpResponseMessage Put(HttpRequestMessage request, ApplicationUser appGroup)
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
                    var result = _appUser.Update(appGroup);
                    if (result.IsSuccessStatusCode)
                    {
                        var newDmNhanvien = result.Content.ReadAsAsync<ApplicationUserViewModel>().Result;
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
                    var result = _appUser.Delete(id);
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
                    var result = _appUser.DeleteMulti(listId);
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