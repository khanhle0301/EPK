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
    [RoutePrefix("api/vethang")]
    public class VeThangController : ApiControllerBase
    {
        private readonly IVeThangService _veThangService;
        private readonly IDmVeThangService _dmVeThangService;

        public VeThangController(IVeThangService veThangService,
             IDmVeThangService dmVeThangService)
        {
            _veThangService = veThangService;
            _dmVeThangService = dmVeThangService;
        }

        [Route("lichsuvethang")]
        [HttpGet]
        public HttpResponseMessage GetXeHuy(HttpRequestMessage request, int page, int pageSize,
            string batDau, string ketThuc, string loai)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage result = null;

                if (loai == "xedangky")
                {
                    result = _veThangService.GetXeDangKy(batDau, ketThuc);
                }
                else
                {
                    result = _veThangService.GetXeHuy(batDau, ketThuc);
                }

                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                var model = result.Content.ReadAsAsync<IEnumerable<VeThangViewModel>>().Result;

                List<LichSuVeThangViewModel> listData = new List<LichSuVeThangViewModel>();
                int i = 0;
                foreach (var item in model)
                {
                    var lichSu = new LichSuVeThangViewModel();
                    i++;
                    lichSu.STT = i;
                    lichSu.BienSo = item.BienSo;
                    lichSu.HoTen = item.HoTen;
                    lichSu.NgayDangKy = item.NgayDangKy.Value.ToString("dd/MM/yyyy HH:mm");
                    lichSu.GiaVe = item.GiaVe.Value.ToString("#,##");
                    lichSu.LoaiXe = item.LoaiXe;
                    lichSu.DanhMuc = _dmVeThangService.GetById(item.DanhMucId.Value).Content.ReadAsAsync<DmVeThangViewModel>().Result.Ten;
                    listData.Add(lichSu);
                }

                int totalRow = listData.Count();

                var query = listData.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                PaginationSet<LichSuVeThangViewModel> pagedSet = new PaginationSet<LichSuVeThangViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = query
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }
}