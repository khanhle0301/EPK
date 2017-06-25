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
    [RoutePrefix("api/thongkenhanh")]
    public class ThongKeNhanhController : ApiControllerBase
    {
        private readonly IThongKeNhanhService _thongKeNhanhService;

        private readonly IGiaHanService _giaHanService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="giaHanService"></param>
        public ThongKeNhanhController(IThongKeNhanhService thongKeNhanhService,
            IGiaHanService giaHanService)
        {
            _thongKeNhanhService = thongKeNhanhService;
            _giaHanService = giaHanService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
            string batDau, string ketThuc, bool vanglai, bool thang, string sort)
        {
            return CreateHttpResponse(request, () =>
            {
                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                var resultRa = _thongKeNhanhService.GetAll(batDau, ketThuc);

                var resultGiaHan = _giaHanService.GetAll(batDau, ketThuc);

                if (!resultRa.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                }

                if (!resultGiaHan.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(resultGiaHan.StatusCode, resultGiaHan.Content.ReadAsStringAsync().Result);
                }

                IEnumerable<Ra> lst_ra = resultRa.Content.ReadAsAsync<IEnumerable<Ra>>().Result.ToList();

                List<GiaHan> lst_giahan = resultGiaHan.Content.ReadAsAsync<IEnumerable<GiaHan>>().Result.ToList();

                List<ThongKeNhanViewModel> listGiaHanVm = new List<ThongKeNhanViewModel>();

                int STT = 1;
                switch (sort)
                {
                    case "ngay":
                        do
                        {
                            ThongKeNhanViewModel giaHanVm = new ThongKeNhanViewModel();
                            giaHanVm.STT = STT;
                            STT++;
                            giaHanVm.MoTa = "Ngày " + _batdau.ToString("dd/MM/yyyy");
                            string _luot = "";
                            int _tongluot = 0;
                            long _tongtien = 0;
                            if (vanglai)
                            {
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId > 0)).Count();
                                _luot += _count + " vãng lai";
                                _tongluot += _count;
                                _tongtien += lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            if (thang)
                            {
                                if (_luot != "")
                                {
                                    _luot += ", ";
                                }
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId < 0)).Count();
                                _luot += _count + " vé tháng";
                                _tongluot += _count;
                                _tongtien += lst_giahan.Where(_gh => (_gh.NgayGiaHan.Value.Day == _batdau.Day) && (_gh.NgayGiaHan.Value.Month == _batdau.Month) && (_gh.NgayGiaHan.Value.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            giaHanVm.SoLuotVao = _luot;
                            giaHanVm.TongTien = _tongtien;
                            if (_tongluot != 0)
                            {
                                giaHanVm.TrungBinh = Convert.ToInt64(_tongtien / _tongluot);
                            }
                            else
                            {
                                giaHanVm.TrungBinh = 0;
                            }

                            listGiaHanVm.Add(giaHanVm);

                            _batdau = _batdau.AddDays(1);
                        } while (_batdau <= _ketthuc);
                        break;

                    case "thang":
                        do
                        {
                            ThongKeNhanViewModel giaHanVm = new ThongKeNhanViewModel();
                            giaHanVm.STT = STT;
                            STT++;
                            giaHanVm.MoTa = "Từ ngày:  " + _batdau.ToString("dd/MM/yyyy") + " đến ngày: " + (new DateTime(_batdau.Year, _batdau.Month, DateTime.DaysInMonth(_batdau.Year, _batdau.Month))).ToString("dd/MM/yyyy");
                            string _luot = "";
                            int _tongluot = 0;
                            long _tongtien = 0;
                            if (vanglai)
                            {
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId > 0)).Count();
                                _luot += _count + " vãng lai";
                                _tongluot += _count;
                                _tongtien += lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            if (thang)
                            {
                                if (_luot != "")
                                {
                                    _luot += ", ";
                                }
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId < 0)).Count();
                                _luot += _count + " vé tháng";
                                _tongluot += _count;
                                _tongtien += lst_giahan.Where(_gh => (_gh.NgayGiaHan.Value.Day == _batdau.Day) && (_gh.NgayGiaHan.Value.Month == _batdau.Month) && (_gh.NgayGiaHan.Value.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            giaHanVm.SoLuotVao = _luot;
                            giaHanVm.TongTien = _tongtien;
                            if (_tongluot != 0)
                            {
                                giaHanVm.TrungBinh = Convert.ToInt64(_tongtien / _tongluot);
                            }
                            else
                            {
                                giaHanVm.TrungBinh = 0;
                            }

                            listGiaHanVm.Add(giaHanVm);

                            _batdau = _batdau.AddDays(-_batdau.Day + 1);
                            _batdau = _batdau.AddMonths(1);
                        } while (_batdau <= _ketthuc);
                        break;

                    case "nam":
                        do
                        {
                            ThongKeNhanViewModel giaHanVm = new ThongKeNhanViewModel();
                            giaHanVm.STT = STT;
                            STT++;
                            giaHanVm.MoTa = "Từ ngày:  " + _batdau.ToString("dd/MM/yyyy") + " đến ngày: " + (new DateTime(_batdau.Year, 12, 31)).ToString("dd/MM/yyyy");
                            string _luot = "";
                            int _tongluot = 0;
                            long _tongtien = 0;
                            if (vanglai)
                            {
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId > 0)).Count();
                                _luot += _count + " vãng lai";
                                _tongluot += _count;
                                _tongtien += lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            if (thang)
                            {
                                if (_luot != "")
                                {
                                    _luot += ", ";
                                }
                                int _count = lst_ra.Where(_ra => (_ra.GioRa.Day == _batdau.Day) && (_ra.GioRa.Month == _batdau.Month) && (_ra.GioRa.Year == _batdau.Year) && (_ra.LoaiVeId < 0)).Count();
                                _luot += _count + " vé tháng";
                                _tongluot += _count;
                                _tongtien += lst_giahan.Where(_gh => (_gh.NgayGiaHan.Value.Day == _batdau.Day) && (_gh.NgayGiaHan.Value.Month == _batdau.Month) && (_gh.NgayGiaHan.Value.Year == _batdau.Year)).Sum(_ra => _ra.GiaVe).Value;
                            }
                            giaHanVm.SoLuotVao = _luot;
                            giaHanVm.TongTien = _tongtien;
                            if (_tongluot != 0)
                            {
                                giaHanVm.TrungBinh = Convert.ToInt64(_tongtien / _tongluot);
                            }
                            else
                            {
                                giaHanVm.TrungBinh = 0;
                            }

                            listGiaHanVm.Add(giaHanVm);

                            _batdau = _batdau.AddDays(-_batdau.Day + 1);
                            _batdau = _batdau.AddMonths(-_batdau.Month + 1);
                            _batdau = _batdau.AddYears(1);
                        } while (_batdau <= _ketthuc);
                        break;
                }
                int totalRow = listGiaHanVm.Count();

                var query = listGiaHanVm.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                PaginationSet<ThongKeNhanViewModel> pagedSet = new PaginationSet<ThongKeNhanViewModel>()
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