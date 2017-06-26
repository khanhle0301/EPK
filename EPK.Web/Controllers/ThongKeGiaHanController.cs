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

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/thongkegiahan")]
    public class ThongKeGiaHanController : ApiControllerBase
    {
        private readonly IThongKeGiaHanService _thongKeGiaHanService;

        private readonly IVeThangService _veThangService;

        private readonly IApplicationUserService _appUser;

        public ThongKeGiaHanController(IThongKeGiaHanService thongKeGiaHanService,
            IVeThangService veThangService, IApplicationUserService appUser)
        {
            _thongKeGiaHanService = thongKeGiaHanService;
            _veThangService = veThangService;
            _appUser = appUser;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
            string batDau, string ketThuc)
        {
            return CreateHttpResponse(request, () =>
            {
                try
                {
                    HttpContext.Current.Session[CommonConstants.SessionSumThongKeGiaHan] = new SumThongKeGiaHan();

                    HttpContext.Current.Session[CommonConstants.SessionThongKeGiaHan] = new List<ThongKeGiaHanViewModel>();

                    List<long> lstLoaiGiaVe = new List<long>();

                    DateTime _batdau = Convert.ToDateTime(batDau);
                    DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                    var resultGiaHan = _thongKeGiaHanService.GetAll(batDau, ketThuc);

                    if (!resultGiaHan.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultGiaHan.StatusCode, resultGiaHan.Content.ReadAsStringAsync().Result);
                    }

                    var resultVeThang = _veThangService.GetAll();

                    if (!resultVeThang.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultVeThang.StatusCode, resultVeThang.Content.ReadAsStringAsync().Result);
                    }

                    var resultNhanVien = _appUser.GetAll();

                    if (!resultNhanVien.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultNhanVien.StatusCode, resultNhanVien.Content.ReadAsStringAsync().Result);
                    }

                    var lst_nhanvien = resultNhanVien.Content.ReadAsAsync<IEnumerable<ApplicationUserViewModel>>().Result.ToList();

                    List<VeThang> lst_vethang = resultVeThang.Content.ReadAsAsync<IEnumerable<VeThang>>().Result.ToList();

                    List<GiaHan> lst_giahan = resultGiaHan.Content.ReadAsAsync<IEnumerable<GiaHan>>().Result.ToList();

                    List<ThongKeGiaHanViewModel> listData = new List<ThongKeGiaHanViewModel>();

                    int i = 0;
                    long _tongtien = 0;
                    foreach (GiaHan _giahan in lst_giahan)
                    {
                        if (lst_vethang.Select(_vt => _vt.Id).Contains(_giahan.VeThangId.Value))
                        {
                            VeThang _vethang = lst_vethang.Single(_vt => _vt.Id == _giahan.VeThangId);
                            i++;
                            var tkGiaHan = new ThongKeGiaHanViewModel();

                            tkGiaHan.STT = i;
                            tkGiaHan.MaThe = _giahan.MaThe;
                            tkGiaHan.BienSo = _giahan.BienSo;
                            tkGiaHan.NgayGiaHan = _giahan.NgayGiaHan.Value.ToString("HH:mm dd/MM/yyyy");
                            tkGiaHan.GiaHanHoTen = _giahan.HoTen;
                            tkGiaHan.VeThangHoTen = _vethang.HoTen;
                            tkGiaHan.GiaVe = _giahan.GiaVe.Value.ToString("#,##");
                            _tongtien = _tongtien + _giahan.GiaVe.Value;
                            if (!lstLoaiGiaVe.Contains(_giahan.GiaVe.Value))
                            {
                                lstLoaiGiaVe.Add(_giahan.GiaVe.Value);
                            }
                            tkGiaHan.NgayHetHan = _vethang.NgayHetHan.Value.ToString("HH:mm dd/MM/yyyy");
                            tkGiaHan.NhanVien = string.IsNullOrEmpty(_giahan.NhanVienId) ? "Không đăng nhập" : lst_nhanvien.Where(_nv => _nv.Id == _giahan.NhanVienId).SingleOrDefault().FullName;
                            listData.Add(tkGiaHan);
                        }
                    }

                    long soLuotXe, tonTien;

                    List<ListLoaiGiaVe> listLoaiGiaVe = new List<ListLoaiGiaVe>();
                    soLuotXe = i;

                    foreach (long item in lstLoaiGiaVe)
                    {
                        var loaiGiaVe = new ListLoaiGiaVe();
                        loaiGiaVe.Name = " + " + lst_giahan.Where(_gh => _gh.GiaVe == item).Count() + " x " + item.ToString("#,## VNĐ");
                        loaiGiaVe.TongTien = (lst_giahan.Where(_gh => _gh.GiaVe == item).Count() * item).ToString("#,## VNĐ");
                        listLoaiGiaVe.Add(loaiGiaVe);
                    }
                    tonTien = _tongtien;

                    int totalRow = listData.Count();

                    var query = listData.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                    var sumTk = new SumThongKeGiaHan
                    {
                        SoLuotXe = soLuotXe.ToString("#,##"),
                        ListLoaiGiaVe = listLoaiGiaVe,
                        TongIien = tonTien.ToString("#,## VNĐ")
                    };

                    PaginationSet<ThongKeGiaHanViewModel> pagedSet = new PaginationSet<ThongKeGiaHanViewModel>()
                    {
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                        Items = query,
                        SumThongKeGiaHan = sumTk
                    };

                    HttpContext.Current.Session[CommonConstants.SessionSumThongKeGiaHan] = sumTk;
                    HttpContext.Current.Session[CommonConstants.SessionThongKeGiaHan] = listData;

                    var response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
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
                    string fileName = string.Concat("DanhSachGiaHan" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
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
                byte[] dsgiahan_bytes = Properties.Resources.DanhSachGiaHan;
                System.IO.File.WriteAllBytes(path, dsgiahan_bytes);
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                xlWorksheet.Cells[4, 1] = "Từ ngày " + _batdau.ToString("dd/MM/yyyy") + " đến ngày " + _ketthuc.ToString("dd/MM/yyyy");
                xlWorksheet.Cells[1, 1] = "Địa chỉ: 279/006C Âu Cơ, P.5, Q.11, Tp.HCM, Việt Nam";
                xlWorksheet.Cells[2, 1] = "Tên: Cty An Phu Viet";

                int rowindex = 0;
                var listData = (List<ThongKeGiaHanViewModel>)HttpContext.Current.Session[CommonConstants.SessionThongKeGiaHan];

                var sumTk = (SumThongKeGiaHan)HttpContext.Current.Session[CommonConstants.SessionSumThongKeGiaHan];

                listData.Add(new ThongKeGiaHanViewModel
                {
                    NgayGiaHan = "Số lượt xe",
                    GiaVe = sumTk.SoLuotXe
                });

                foreach (var item in sumTk.ListLoaiGiaVe)
                {
                    listData.Add(new ThongKeGiaHanViewModel
                    {
                        NgayGiaHan = item.Name,
                        GiaVe = item.TongTien
                    });
                }

                listData.Add(new ThongKeGiaHanViewModel
                {
                    NgayGiaHan = "Tổng tiền",
                    GiaVe = sumTk.TongIien.ToString()
                });

                var data = new object[listData.Count, 8];
                foreach (var item in listData)
                {
                    data[rowindex, 0] = item.STT;
                    data[rowindex, 1] = item.BienSo == null ? "" : item.BienSo;
                    data[rowindex, 2] = item.NgayGiaHan == null ? "" : item.NgayGiaHan;
                    data[rowindex, 3] = item.GiaHanHoTen == null ? "" : item.GiaHanHoTen;
                    data[rowindex, 4] = item.VeThangHoTen == null ? "" : item.VeThangHoTen;
                    data[rowindex, 5] = item.GiaVe == null ? "" : item.GiaVe;
                    data[rowindex, 6] = item.NgayHetHan == null ? "" : item.NgayHetHan;
                    data[rowindex, 7] = item.NhanVien == null ? "" : item.NhanVien;
                    rowindex++;
                }

                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.Range[xlWorksheet.Cells[7, 1], xlWorksheet.Cells[7 + listData.Count, 8]];
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
            catch (Exception)
            {
            }
        }
    }
}