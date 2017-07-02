using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/timkiem")]
    public class TimKiemController : ApiControllerBase
    {
        private readonly IRaService _raService;
        private readonly IVaoService _vaoService;
        private readonly IHinhAnhVaoService _hinhAnhVaoService;

        public TimKiemController(IRaService raService,
            IVaoService vaoService,
             IHinhAnhVaoService hinhAnhVaoService)
        {
            _raService = raService;
            _vaoService = vaoService;
            _hinhAnhVaoService = hinhAnhVaoService;
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        [Route("showimage")]
        [HttpGet]
        public HttpResponseMessage ShowImage(HttpRequestMessage request, string maHinhAnh)
        {
            return CreateHttpResponse(request, () =>
            {
                var arr = maHinhAnh.Split('-');
                string MaThe = arr[0].ToString();
                string GioVao = Convert.ToDateTime(arr[1]).ToString("ddMMyyyyHHmmssfff");
                var result = _hinhAnhVaoService.GetVaoMaHinhAnh(MaThe + "." + GioVao);

                if (!result.IsSuccessStatusCode)
                {
                    return request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                }

                List<HinhAnhVao> lst_hinhanh_vao = result.Content.ReadAsAsync<IEnumerable<HinhAnhVao>>().Result.ToList();

                List<HinhAnhViewModel> listData = new List<HinhAnhViewModel>();

                foreach (var item in lst_hinhanh_vao)
                {
                    var hinhAnh = new HinhAnhViewModel();
                    hinhAnh.Id = item.Id;
                    hinhAnh.HinhAnh = Convert.FromBase64String(item.HinhAnh);
                    listData.Add(hinhAnh);
                }
                var response = request.CreateResponse(HttpStatusCode.OK, listData);
                return response;
            });
        }

        [Route("timkiem")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
            string maThe, string bienSo,
            string batDau, string ketThuc, bool trongbai, bool ngoaibai, string thoigian,
            bool xethang, bool vanglai)
        {
            return CreateHttpResponse(request, () =>
            {
                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);
                List<TimKiemViewModel> listData;

                if (trongbai)
                {
                    listData = new List<TimKiemViewModel>();
                    var resultVao = _vaoService.GetVaoTimKiem(maThe, bienSo, batDau, ketThuc);

                    if (!resultVao.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultVao.StatusCode, resultVao.Content.ReadAsStringAsync().Result);
                    }

                    List<Vao> lst_vao = resultVao.Content.ReadAsAsync<IEnumerable<Vao>>().Result.ToList();

                    if (!vanglai)
                    {
                        lst_vao = lst_vao.Where(_vao => _vao.LoaiVeId < 0).ToList();
                    }
                    if (!xethang)
                    {
                        lst_vao = lst_vao.Where(_vao => _vao.LoaiVeId > 0).ToList();
                    }

                    foreach (Vao item in lst_vao)
                    {
                        var timKiem = new TimKiemViewModel();
                        timKiem.MaThe = item.MaThe;
                        timKiem.BienSo = item.BienSo;
                        timKiem.GioVao = item.GioVao.Value.ToString("dd/MM/yyyy HH:mm:ss.fff");
                        timKiem.GioRa = "Xe còn trong bãi";
                        listData.Add(timKiem);
                    }
                }
                else
                {
                    listData = new List<TimKiemViewModel>();
                }
                if (ngoaibai)
                {
                    if (thoigian == "ra")
                    {
                        var resultRa = _raService.GetRaTheoGioRa(maThe, bienSo, batDau, ketThuc);

                        if (!resultRa.IsSuccessStatusCode)
                        {
                            return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                        }

                        List<Ra> lst_ra = resultRa.Content.ReadAsAsync<IEnumerable<Ra>>().Result.ToList();

                        if (!vanglai)
                        {
                            lst_ra = lst_ra.Where(_vao => _vao.LoaiVeId < 0).ToList();
                        }
                        if (!xethang)
                        {
                            lst_ra = lst_ra.Where(_vao => _vao.LoaiVeId > 0).ToList();
                        }
                        foreach (var item in lst_ra)
                        {
                            var timKiem = new TimKiemViewModel();
                            timKiem.MaThe = item.MaThe;
                            timKiem.BienSo = item.BienSo;
                            timKiem.GioVao = item.GioVao.Value.ToString("dd/MM/yyyy HH:mm:ss.fff");
                            timKiem.GioRa = item.GioRa.ToString("dd/MM/yyyy HH:mm");
                            listData.Add(timKiem);
                        }
                    }
                    else
                    {
                        var resultRa = _raService.GetRaTheoGioVao(maThe, bienSo, batDau, ketThuc);

                        if (!resultRa.IsSuccessStatusCode)
                        {
                            return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                        }

                        List<Ra> lst_ra = resultRa.Content.ReadAsAsync<IEnumerable<Ra>>().Result.ToList();
                        if (!vanglai)
                        {
                            lst_ra = lst_ra.Where(_vao => _vao.LoaiVeId < 0).ToList();
                        }
                        if (!xethang)
                        {
                            lst_ra = lst_ra.Where(_vao => _vao.LoaiVeId > 0).ToList();
                        }

                        foreach (var item in lst_ra)
                        {
                            var timKiem = new TimKiemViewModel();
                            timKiem.MaThe = item.MaThe;
                            timKiem.BienSo = item.BienSo;
                            timKiem.GioVao = item.GioVao.Value.ToString("dd/MM/yyyy HH:mm:ss.fff");
                            timKiem.GioRa = item.GioRa.ToString("dd/MM/yyyy HH:mm:ss");
                            listData.Add(timKiem);
                        }
                    }
                }
                int totalRow = listData.Count();
                var query = listData.OrderBy(x => x.GioVao).Skip(page * pageSize).Take(pageSize);

                PaginationSet<TimKiemViewModel> pagedSet = new PaginationSet<TimKiemViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = query,
                };

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }
}