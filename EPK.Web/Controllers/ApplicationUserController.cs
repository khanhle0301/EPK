using AutoMapper;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
using System;
using System.Collections.Generic;
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
            //return CreateHttpResponse(request, () =>
            //{
            //    var model = _appUser.GetAll();
            //    var modelVm = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

            //    int totalRow = 0;
            //    PaginationSet<ApplicationUserViewModel> pagedSet = new PaginationSet<ApplicationUserViewModel>()
            //    {
            //        Page = page,
            //        TotalCount = totalRow,
            //        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
            //        Items = modelVm
            //    };

            //    var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

            //    return response;
            //});
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
    }
}