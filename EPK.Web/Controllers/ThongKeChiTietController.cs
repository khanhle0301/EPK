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

namespace EPK.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/thongkechitiet")]
    public class ThongKeChiTietController : ApiControllerBase
    {
        private readonly IThongKeNhanhService _thongKeNhanhService;

        private readonly ILoaiVeService _loaiVeService;

        private readonly IApplicationUserService _appUser;

        private readonly IVaoService _vaoService;

        private readonly ILoaiXeService _loaiXeService;

        public ThongKeChiTietController(IThongKeNhanhService thongKeNhanhService,
            ILoaiVeService loaiVeService, IApplicationUserService appUser,
            IVaoService vaoService, ILoaiXeService loaiXeService)
        {
            _thongKeNhanhService = thongKeNhanhService;
            _loaiVeService = loaiVeService;
            _appUser = appUser;
            _vaoService = vaoService;
            _loaiXeService = loaiXeService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize,
            string batDau, string ketThuc, bool vanglai, bool thang, bool sendmail, string trongbai)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpContext.Current.Session[CommonConstants.SessionSumThongKeChiTiet] = new SumThongKeChiTiet();

                HttpContext.Current.Session[CommonConstants.SessionThongKeChiTiet] = new List<ThongKeChiTietViewModel>();

                HttpContext.Current.Session[CommonConstants.SessionListRa] = new List<Ra>();

                try
                {
                    var resultRa = _thongKeNhanhService.GetAll(batDau, ketThuc);

                    if (!resultRa.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                    }

                    var resultLoaive = _loaiVeService.GetAll();

                    if (!resultLoaive.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                    }

                    var resultNhanVien = _appUser.GetAll();

                    if (!resultNhanVien.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultNhanVien.StatusCode, resultNhanVien.Content.ReadAsStringAsync().Result);
                    }

                    var resultLoaXe = _loaiXeService.GetAll();

                    if (!resultLoaXe.IsSuccessStatusCode)
                    {
                        return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                    }

                    var lst_loaixe = resultLoaXe.Content.ReadAsAsync<IEnumerable<LoaiXe>>().Result.ToList();

                    var lst_nhanvien = resultNhanVien.Content.ReadAsAsync<IEnumerable<ApplicationUserViewModel>>().Result.ToList();

                    var lst_loaive = resultLoaive.Content.ReadAsAsync<IEnumerable<LoaiVe>>().Result.ToList();

                    IEnumerable<Ra> lst_ra = resultRa.Content.ReadAsAsync<IEnumerable<Ra>>().Result.ToList();

                    List<ThongKeChiTietViewModel> listData = new List<ThongKeChiTietViewModel>();

                    int soLuotXe, xeVangLai, xeThang;
                    List<ListXe> listXe = new List<ListXe>();

                    if (trongbai != "trongbai")
                    {
                        if (!thang)
                        {
                            lst_ra = lst_ra.Where(_ra => _ra.LoaiVeId > 0).ToList();
                        }
                        if (!vanglai)
                        {
                            lst_ra = lst_ra.Where(_ra => _ra.LoaiVeId < 0).ToList();
                        }
                        int i = 0;
                        long _tongtien = 0;
                        foreach (Ra _ra in lst_ra)
                        {
                            ThongKeChiTietViewModel tkChiTiet = new ThongKeChiTietViewModel();
                            i++;
                            tkChiTiet.STT = i;
                            tkChiTiet.MaThe = _ra.MaThe;
                            tkChiTiet.BienSo = _ra.BienSo;
                            tkChiTiet.GioVao = _ra.GioVao.ToString();
                            tkChiTiet.GioRa = _ra.GioRa.ToString();
                            tkChiTiet.GiaVe = _ra.GiaVe.ToString();
                            _tongtien += _ra.GiaVe.Value;

                            if (_ra.LoaiVeId < 0)
                            {
                                tkChiTiet.LoaiVe = "Vé tháng";
                            }
                            else
                            {
                                LoaiVe loaive = lst_loaive.Where(_lx => _lx.Id == _ra.LoaiVeId).SingleOrDefault();
                                if (loaive != null)
                                    tkChiTiet.LoaiVe = loaive.Ten;
                                else tkChiTiet.LoaiVe = "Đã xóa";
                            }

                            if (string.IsNullOrEmpty(_ra.NhanVienId_Vao))
                            {
                                tkChiTiet.NhanVienVao = "Không đăng nhập";
                            }
                            else
                            {
                                if (lst_nhanvien != null)
                                {
                                    ApplicationUserViewModel nv = lst_nhanvien.Where(_mt => _mt.Id == _ra.NhanVienId_Vao).SingleOrDefault();
                                    if (nv != null)
                                    {
                                        tkChiTiet.NhanVienVao = nv.FullName;
                                    }
                                    else
                                    {
                                        tkChiTiet.NhanVienVao = "Đã xóa";
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(_ra.NhanVienId_Ra))
                            {
                                tkChiTiet.NhanVienRa = "Không đăng nhập";
                            }
                            else
                            {
                                if (lst_nhanvien != null)
                                {
                                    ApplicationUserViewModel nv = lst_nhanvien.Where(_mt => _mt.Id == _ra.NhanVienId_Ra).SingleOrDefault();
                                    if (nv != null)
                                    {
                                        tkChiTiet.NhanVienRa = nv.FullName;
                                    }
                                    else
                                    {
                                        tkChiTiet.NhanVienRa = "Đã xóa";
                                    }
                                }
                            }
                            listData.Add(tkChiTiet);
                        }

                        soLuotXe = i;
                        xeVangLai = lst_ra.Where(_ra => _ra.LoaiVeId > 0).Count();

                        foreach (LoaiXe _loaixe in lst_loaixe)
                        {
                            var list = new ListXe();
                            list.Id = _loaixe.Id;
                            list.Name = _loaixe.Ten;
                            list.Count = lst_ra.Where(_ra => _ra.LoaiVeId == _loaixe.LoaiVeId).Count();
                            listXe.Add(list);
                        }
                        xeThang = lst_ra.Where(_ra => _ra.LoaiVeId < 0).Count();
                    }
                    else
                    {
                        var resultVao = _vaoService.GetTimKiem(batDau, ketThuc);

                        if (!resultVao.IsSuccessStatusCode)
                        {
                            return request.CreateErrorResponse(resultRa.StatusCode, resultRa.Content.ReadAsStringAsync().Result);
                        }

                        List<Vao> lst_vao = resultVao.Content.ReadAsAsync<IEnumerable<Vao>>().Result.ToList();
                        if (!thang)
                        {
                            lst_vao = lst_vao.Where(_vao => _vao.LoaiVeId > 0).ToList();
                        }
                        if (!vanglai)
                        {
                            lst_vao = lst_vao.Where(_vao => _vao.LoaiVeId < 0).ToList();
                        }

                        int i = 0;
                        foreach (Vao _vao in lst_vao)
                        {
                            ThongKeChiTietViewModel tkChiTiet = new ThongKeChiTietViewModel();
                            i++;
                            tkChiTiet.STT = i;
                            tkChiTiet.MaThe = _vao.MaThe;
                            tkChiTiet.BienSo = _vao.BienSo;
                            tkChiTiet.GioVao = _vao.GioVao.ToString();
                            tkChiTiet.GioRa = "Xe trong bãi";
                            LoaiVe lv = lst_loaive.Where(_lx => _lx.Id == _vao.LoaiVeId).SingleOrDefault();
                            if (lv != null)
                            {
                                LoaiXe lx = lst_loaixe.Where(_lx => _lx.LoaiVeId == _vao.LoaiVeId).SingleOrDefault();
                                if (lx != null)
                                {
                                    if (lx.ThuTienTruoc.Value)
                                    {
                                        // tkChiTiet.GiaVe = lv.tinhveTNXP(_vao.GioVao, _vao.GioVao).ToString("#,##0 VNĐ");
                                        tkChiTiet.GiaVe = "0";
                                    }
                                    else
                                    {
                                        tkChiTiet.GiaVe = "Xe trong bãi";
                                    }
                                }
                            }
                            else
                            {
                                tkChiTiet.GiaVe = "Loại Vé đã xóa";
                            }
                            if (_vao.LoaiVeId < 0)
                            {
                                tkChiTiet.LoaiVe = "Vé tháng";
                            }
                            else
                            {
                                if (lv != null)
                                {
                                    tkChiTiet.LoaiVe = lv.Ten;
                                }
                                else
                                {
                                    tkChiTiet.LoaiVe = "Loại Vé đã xóa";
                                }
                            }
                            if (string.IsNullOrEmpty(_vao.NhanVienId))
                            {
                                tkChiTiet.NhanVienVao = "Không đăng nhập";
                            }
                            else
                            {
                                if (lst_nhanvien != null)
                                {
                                    ApplicationUserViewModel nv = lst_nhanvien.Where(_mt => _mt.Id == _vao.NhanVienId).SingleOrDefault();
                                    if (nv != null)
                                    {
                                        tkChiTiet.NhanVienVao = nv.FullName;
                                    }
                                    else
                                    {
                                        tkChiTiet.NhanVienVao = "Đã xóa nv";
                                    }
                                }
                            }
                            tkChiTiet.NhanVienRa = "Xe trong bãi";
                            listData.Add(tkChiTiet);
                        }

                        soLuotXe = i;
                        xeVangLai = lst_vao.Where(_vao => _vao.LoaiVeId > 0).Count();

                        foreach (LoaiXe _loaixe in lst_loaixe)
                        {
                            var list = new ListXe();
                            list.Id = _loaixe.Id;
                            list.Name = _loaixe.Ten;
                            list.Count = lst_vao.Where(_vao => _vao.LoaiVeId == _loaixe.LoaiVeId).Count();
                            listXe.Add(list);
                        }
                        xeThang = lst_vao.Where(_vao => _vao.LoaiVeId < 0).Count();
                    }
                    int totalRow = listData.Count();

                    var query = listData.OrderBy(x => x.STT).Skip(page * pageSize).Take(pageSize);

                    var sumTk = new SumThongKeChiTiet
                    {
                        SoLuotXe = soLuotXe,
                        XeVangLai = xeVangLai,
                        ListXeVangLai = listXe,
                        XeThang = xeThang
                    };

                    PaginationSet<ThongKeChiTietViewModel> pagedSet = new PaginationSet<ThongKeChiTietViewModel>()
                    {
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                        Items = query,
                        SumThongKeChiTiet = sumTk
                    };

                    HttpContext.Current.Session[CommonConstants.SessionSumThongKeChiTiet] = sumTk;
                    HttpContext.Current.Session[CommonConstants.SessionThongKeChiTiet] = listData;

                    HttpContext.Current.Session[CommonConstants.SessionListRa] = lst_ra.ToList();

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
        public HttpResponseMessage ExportXls(HttpRequestMessage request, string batDau, string ketThuc,
            string type, string trongbai, bool sendmail)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                try
                {
                    string fileName = string.Empty;
                    if (type == "chitiet")
                    {
                        fileName = string.Concat("ThongKeChiTiet_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
                    }
                    else
                    {
                        fileName = string.Concat("ThongKeTongQuat_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
                    }
                    var folderReport = ConfigHelper.GetByKey("ReportFolder");
                    string filePath = HttpContext.Current.Server.MapPath(folderReport);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fullPath = Path.Combine(filePath, fileName);

                    if (type == "chitiet")
                    {
                        _f_excel(fullPath, batDau, ketThuc, trongbai);
                    }
                    else
                    {
                        _f_excel_tongquat(fullPath, batDau, ketThuc);
                    }

                    if (sendmail)
                    {
                        MailHelper.SendMail("khanhle0301.it@gmail.com", "Báo cáo chi tiết", "Báo cáo chi tiết ngày_" + DateTime.Now, fullPath);
                    }

                    response = request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
                }
                catch (Exception ex)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
                return response;
            });
        }

        private void _f_excel_tongquat(string path, string batDau, string ketThuc)
        {
            try
            {
                byte[] thongke_bytes = Properties.Resources.ThongKe_TongQuat;
                File.WriteAllBytes(path, thongke_bytes);
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                var resultLoaXe = _loaiXeService.GetAll();

                var lst_loaixe = resultLoaXe.Content.ReadAsAsync<IEnumerable<LoaiXe>>().Result.ToList();

                var lst_ra = (List<Ra>)HttpContext.Current.Session[CommonConstants.SessionListRa];

                int row_index = 0;
                var data = new object[lst_loaixe.Count, 4];
                foreach (LoaiXe _xe in lst_loaixe)
                {
                    data[row_index, 0] = _xe.Ten;
                    data[row_index, 2] = lst_ra.Where(_ra => _ra.LoaiVeId == _xe.LoaiVeId).Count();
                    data[row_index, 3] = lst_ra.Where(_ra => _ra.LoaiVeId == _xe.LoaiVeId).Sum(_ra => _ra.GiaVe);
                    row_index++;
                }

                Range xlRange = xlWorksheet.Range[xlWorksheet.Cells[7, 2], xlWorksheet.Cells[7 + lst_loaixe.Count, 6]];
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

        private void _f_excel(string path, string batDau, string ketThuc, string trongbai)
        {
            try
            {
                byte[] thongke_bytes = Properties.Resources.ThongKe;
                File.WriteAllBytes(path, thongke_bytes);
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(path);
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                if (trongbai == "trongbai")
                {
                    xlWorksheet.Cells[3, 1] = "DANH SÁCH XE TRONG BÃI";
                }
                else
                {
                    xlWorksheet.Cells[3, 1] = "DANH SÁCH XE NGOÀI BÃI";
                }

                DateTime _batdau = Convert.ToDateTime(batDau);
                DateTime _ketthuc = Convert.ToDateTime(ketThuc);

                xlWorksheet.Cells[4, 1] = "Từ ngày " + _batdau.ToString("dd/MM/yyyy") + " đến ngày " + _ketthuc.ToString("dd/MM/yyyy");
                xlWorksheet.Cells[1, 1] = "Địa chỉ: 279/006C Âu Cơ, P.5, Q.11, Tp.HCM, Việt Nam";
                xlWorksheet.Cells[2, 1] = "Tên: Cty An Phu Viet";
                int rowindex = 0;

                var listData = (List<ThongKeChiTietViewModel>)HttpContext.Current.Session[CommonConstants.SessionThongKeChiTiet];

                var sumTk = (SumThongKeChiTiet)HttpContext.Current.Session[CommonConstants.SessionSumThongKeChiTiet];

                listData.Add(new ThongKeChiTietViewModel
                {
                    BienSo = "Số lượt xe",
                    GioRa = sumTk.SoLuotXe.ToString()
                });
                listData.Add(new ThongKeChiTietViewModel
                {
                    BienSo = "Xe vãng lai",
                    GioRa = sumTk.XeVangLai.ToString()
                });
                foreach (var item in sumTk.ListXeVangLai)
                {
                    listData.Add(new ThongKeChiTietViewModel
                    {
                        BienSo = " - " + item.Name,
                        GioRa = item.Count.ToString()
                    });
                }
                listData.Add(new ThongKeChiTietViewModel
                {
                    BienSo = "Xe tháng",
                    GioRa = sumTk.XeThang.ToString()
                });

                var data = new object[listData.Count, 8];
                foreach (var item in listData)
                {
                    data[rowindex, 0] = item.STT;
                    data[rowindex, 1] = item.BienSo == null ? "" : item.BienSo;
                    data[rowindex, 2] = item.GioVao == null ? "" : item.GioVao;
                    data[rowindex, 3] = item.GioRa == null ? "" : item.GioRa;
                    data[rowindex, 4] = item.GiaVe == null ? "" : item.GiaVe;
                    data[rowindex, 5] = item.LoaiVe == null ? "" : item.LoaiVe;
                    data[rowindex, 6] = item.NhanVienVao == null ? "" : item.NhanVienVao;
                    data[rowindex, 7] = item.NhanVienRa == null ? "" : item.NhanVienRa;
                    rowindex++;
                }

                Range xlRange = xlWorksheet.Range[xlWorksheet.Cells[7, 1], xlWorksheet.Cells[7 + listData.Count, 8]];

                xlRange.Value2 = data;
                xlWorkbook.Save();
                xlWorkbook.Close();

                Marshal.FinalReleaseComObject(xlRange);
                Marshal.FinalReleaseComObject(xlWorksheet);
                Marshal.FinalReleaseComObject(xlWorkbook);

                xlRange = null;
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