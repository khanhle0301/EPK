using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/thongkenhanh")]
    public class ThongKeNhanhController : ApiControllerBase
    {
        private readonly IThongKeNhanhService _thongKeNhanhService;

        private readonly IMayTinhService _nhanVienService;

        private readonly IGiaHanService _giaHanService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="giaHanService"></param>
        public ThongKeNhanhController(IThongKeNhanhService thongKeNhanhService,
            IGiaHanService giaHanService, IMayTinhService nhanVienService)
        {
            _thongKeNhanhService = thongKeNhanhService;
            _giaHanService = giaHanService;
            _nhanVienService = nhanVienService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
            string batDau, string ketThuc, bool vanglai, bool thang, string sort, string maytinh, string nhanvien)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpContext.Current.Session[CommonConstants.SessionThongKeNhanh] = new List<ThongKeNhanViewModel>();

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

                if (!string.IsNullOrEmpty(maytinh))
                {
                    List<string> lst_maytinh_selected = new List<string>();

                    var listItem = new JavaScriptSerializer().Deserialize<List<string>>(maytinh);
                    foreach (var item in listItem)
                    {
                        lst_maytinh_selected.Add(item);
                    }

                    lst_ra = lst_ra.Where(_ra => lst_maytinh_selected.Contains(_ra.MayTinhId_Ra)).ToList();
                    lst_giahan = lst_giahan.Where(_gh => lst_maytinh_selected.Contains(_gh.MayTinhId)).ToList();
                }
                else if (!string.IsNullOrEmpty(nhanvien))
                {
                    List<string> lst_nhanvien_selected = new List<string>();

                    var listItem = new JavaScriptSerializer().Deserialize<List<string>>(nhanvien);
                    foreach (var item in listItem)
                    {
                        lst_nhanvien_selected.Add(item);
                    }

                    lst_ra = lst_ra.Where(_ra => lst_nhanvien_selected.Contains(_ra.NhanVienId_Ra)).ToList();
                    lst_giahan = lst_giahan.Where(_gh => lst_nhanvien_selected.Contains(_gh.NhanVienId)).ToList();
                }

                List<ThongKeNhanViewModel> listData = new List<ThongKeNhanViewModel>();

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

                            listData.Add(giaHanVm);

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

                            listData.Add(giaHanVm);

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

                            listData.Add(giaHanVm);

                            _batdau = _batdau.AddDays(-_batdau.Day + 1);
                            _batdau = _batdau.AddMonths(-_batdau.Month + 1);
                            _batdau = _batdau.AddYears(1);
                        } while (_batdau <= _ketthuc);
                        break;
                }
                int totalRow = listData.Count();

                var query = listData.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                PaginationSet<ThongKeNhanViewModel> pagedSet = new PaginationSet<ThongKeNhanViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = query
                };

                HttpContext.Current.Session[CommonConstants.SessionThongKeNhanh] = listData;

                var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("ExportXls")]
        [HttpGet]
        public HttpResponseMessage ExportXls(HttpRequestMessage request, string batDau, string ketThuc)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                try
                {
                    string fileName = string.Concat("ThongKeNhanh_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
                    var folderReport = ConfigHelper.GetByKey("ReportFolder");
                    string filePath = HttpContext.Current.Server.MapPath(folderReport);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fullPath = Path.Combine(filePath, fileName);

                    _f_excel(fullPath, batDau, ketThuc);
                    response = request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
                }
                catch (Exception ex)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
                return response;
            });
        }

        private void _f_excel(string path, string batDau, string ketThuc)
        {
            try
            {
                byte[] thongke_bytes = Properties.Resources.ThongKe_Nhanh;
                File.WriteAllBytes(path, thongke_bytes);
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                xlWorksheet.Cells[4, 1] = "Từ ngày " + _batdau.ToString("dd/MM/yyyy") + " đến ngày " + _ketthuc.ToString("dd/MM/yyyy");

                xlWorksheet.Cells[1, 1] = "Địa chỉ: 279/006C Âu Cơ, P.5, Q.11, Tp.HCM, Việt Nam";
                xlWorksheet.Cells[2, 1] = "Tên: Cty An Phu Viet";

                int rowindex = 0;

                var listData = (List<ThongKeNhanViewModel>)HttpContext.Current.Session[CommonConstants.SessionThongKeNhanh];

                var data = new object[listData.Count, 4];
                foreach (var item in listData)
                {
                    data[rowindex, 0] = item.STT;
                    data[rowindex, 1] = item.MoTa == null ? "" : item.MoTa;
                    data[rowindex, 2] = item.SoLuotVao == null ? "" : item.SoLuotVao;
                    data[rowindex, 3] = item.TongTien;
                    rowindex++;
                }

                Range xlRange = xlWorksheet.Range[xlWorksheet.Cells[7, 1], xlWorksheet.Cells[7 + listData.Count, 4]];

                xlRange.Value2 = data;
                xlWorkbook.Save();
                xlWorkbook.Close();

                //Marshal.FinalReleaseComObject(xlRange);
                Marshal.FinalReleaseComObject(xlWorksheet);
                Marshal.FinalReleaseComObject(xlWorkbook);

                //xlRange = null;
                xlWorksheet = null;
                xlWorkbook = null;
                xlApp.Quit();
                Marshal.FinalReleaseComObject(xlApp);
                xlApp = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}