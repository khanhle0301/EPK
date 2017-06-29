using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Service;
using EPK.Web.Infrastructure.Core;
using EPK.Web.Models;
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
    [RoutePrefix("api/lichsu")]
    public class LichSuController : ApiControllerBase
    {
        private readonly ILichSuService _lichSuService;
        private readonly IMayTinhService _mayTinhService;
        private readonly IApplicationUserService _appUserService;

        public LichSuController(ILichSuService lichSuService,
            IMayTinhService mayTinhService, IApplicationUserService appUserService)
        {
            _lichSuService = lichSuService;
            _mayTinhService = mayTinhService;
            _appUserService = appUserService;
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request)
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
                    var listData = (List<ThongKeLichSuViewModel>)HttpContext.Current.Session[CommonConstants.SessionLichSu];

                    var listId = new JavaScriptSerializer().Serialize(listData.Select(x => x.Id));

                    var result = _lichSuService.DeleteMulti(listId);

                    if (result.IsSuccessStatusCode)
                    {
                        var lichSu = result.Content.ReadAsAsync<int>().Result;
                        response = request.CreateResponse(result.StatusCode, lichSu);
                        HttpContext.Current.Session[CommonConstants.SessionLichSu] = new ThongKeLichSuViewModel();
                    }
                    else
                    {
                        response = request.CreateErrorResponse(result.StatusCode, result.Content.ReadAsStringAsync().Result);
                    }
                }
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
                string batDau, string ketThuc)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                try
                {
                    HttpContext.Current.Session[CommonConstants.SessionLichSu] = new ThongKeLichSuViewModel();

                    DateTime _batdau = Convert.ToDateTime(batDau);
                    DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                    var resultLichSu = _lichSuService.GetAll(batDau, ketThuc);

                    if (!resultLichSu.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultLichSu.StatusCode, resultLichSu.Content.ReadAsStringAsync().Result);
                    }

                    var resultMayTinh = _mayTinhService.GetAll();

                    if (!resultMayTinh.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultMayTinh.StatusCode, resultMayTinh.Content.ReadAsStringAsync().Result);
                    }

                    var resultNhanVien = _appUserService.GetAll();

                    if (!resultNhanVien.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultNhanVien.StatusCode, resultNhanVien.Content.ReadAsStringAsync().Result);
                    }

                    var lst_nhanvien = resultNhanVien.Content.ReadAsAsync<IEnumerable<ApplicationUserViewModel>>().Result.ToList();

                    List<MayTinh> lst_maytinh = resultMayTinh.Content.ReadAsAsync<IEnumerable<MayTinh>>().Result.ToList();

                    List<LichSu> lst_lichsu = resultLichSu.Content.ReadAsAsync<IEnumerable<LichSu>>().Result.ToList();

                    List<ThongKeLichSuViewModel> listData = new List<ThongKeLichSuViewModel>();

                    int i = 1;
                    foreach (var item in lst_lichsu)
                    {
                        var lichSuVm = new ThongKeLichSuViewModel();

                        lichSuVm.Id = item.Id;
                        lichSuVm.STT = i;
                        lichSuVm.ThoiGian = item.Ngay.Value.ToString("dd/MM/yyyy HH:mm");
                        lichSuVm.MayTinh = lst_maytinh.Where(_mt => _mt.Id == item.MayTinhId).SingleOrDefault().Id;
                        string _nhanvienid = item.NhanVienId;
                        if (string.IsNullOrEmpty(_nhanvienid))
                        {
                            lichSuVm.NhanVien = "Không đăng nhập";
                        }
                        else
                        {
                            lichSuVm.NhanVien = lst_nhanvien.Where(_nv => _nv.Id == _nhanvienid).SingleOrDefault().FullName;
                        }
                        lichSuVm.HanhDong = item.NoiDung;

                        listData.Add(lichSuVm);
                        i++;
                    }

                    int totalRow = listData.Count();
                    var query = listData.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                    PaginationSet<ThongKeLichSuViewModel> pagedSet = new PaginationSet<ThongKeLichSuViewModel>()
                    {
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                        Items = query
                    };
                    HttpContext.Current.Session[CommonConstants.SessionLichSu] = listData;

                    response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                }
                catch (Exception ex)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
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
                    string fileName = string.Concat("LichSu" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
                    var folderReport = ConfigHelper.GetByKey("ReportFolder");
                    string filePath = HttpContext.Current.Server.MapPath(folderReport);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fullPath = Path.Combine(filePath, fileName);

                    f_excel(fullPath, batDau, ketThuc);
                    response = request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
                }
                catch (Exception ex)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
                return response;
            });
        }

        private void f_excel(string path, string batDau, string ketThuc)
        {
            try
            {
                byte[] dsgiahan_bytes = Properties.Resources.LichSu;
                System.IO.File.WriteAllBytes(path, dsgiahan_bytes);
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                xlWorksheet.Cells[4, 1] = "Từ ngày " + _batdau.ToString("dd/MM/yyyy") + " đến ngày " + _ketthuc.ToString("dd/MM/yyyy");
                xlWorksheet.Cells[1, 1] = "Địa chỉ: 279/006C Âu Cơ, P.5, Q.11, Tp.HCM, Việt Nam";
                xlWorksheet.Cells[2, 1] = "Tên: Cty An Phu Viet";

                var listData = (List<ThongKeLichSuViewModel>)HttpContext.Current.Session[CommonConstants.SessionLichSu];

                int rowindex = 0;
                var data = new Object[listData.Count, 5];
                foreach (var item in listData)
                {
                    data[rowindex, 0] = item.STT;
                    data[rowindex, 1] = item.ThoiGian == null ? "" : item.ThoiGian;
                    data[rowindex, 2] = item.MayTinh == null ? "" : item.MayTinh;
                    data[rowindex, 3] = item.NhanVien == null ? "" : item.NhanVien;
                    data[rowindex, 4] = item.HanhDong == null ? "" : item.HanhDong;
                    rowindex++;
                }

                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.Range[xlWorksheet.Cells[7, 1], xlWorksheet.Cells[7 + listData.Count, 5]];
                Microsoft.Office.Interop.Excel.Range xlYourRange = xlWorksheet.Range[xlWorksheet.Cells[7, 2], xlWorksheet.Cells[7 + listData.Count, 2]];
                xlRange.Value2 = data;
                xlYourRange.NumberFormat = "dd/mm/yyyy hh:mm:ss";
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
            catch (Exception)
            {
            }
        }
    }
}