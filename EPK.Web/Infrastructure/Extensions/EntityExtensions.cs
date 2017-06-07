using EPK.Model.Models;

namespace EPK.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateDmNhanVien(this DmNhanVien dmNhanVien, DmNhanVienViewModel dmNhanVienVm)
        {
            dmNhanVien.ID = dmNhanVienVm.ID;
            dmNhanVien.Ten = dmNhanVienVm.Ten;
            dmNhanVien.GhiChu = dmNhanVienVm.GhiChu;
            dmNhanVien.DangSuDung = dmNhanVienVm.DangSuDung;
        }

        public static void UpdateDmVeThang(this DmVeThang dmVeThang, DmVeThangViewModel dmVeThangVm)
        {
            dmVeThang.ID = dmVeThangVm.ID;
            dmVeThang.Ten = dmVeThangVm.Ten;
            dmVeThang.GhiChu = dmVeThangVm.GhiChu;
            dmVeThang.DangSuDung = dmVeThangVm.DangSuDung;
        }

        public static void UpdateVao(this Vao vao, VaoViewModel vaoVm)
        {
            vao.MaThe = vaoVm.MaThe;
            vao.GioVao = vaoVm.GioVao;
            vao.LoaiVeID = vaoVm.LoaiVeID;
            vao.BienSo = vaoVm.BienSo;
            vao.MayTinhID = vaoVm.MayTinhID;
            vao.NhanVienID = vaoVm.NhanVienID;
        }

        public static void UpdateRa(this Ra ra, RaViewModel raVm)
        {
            ra.MaThe = raVm.MaThe;
            ra.BienSo = raVm.BienSo;
            ra.GioVao = raVm.GioVao;
            ra.GioRa = raVm.GioRa;
            ra.GiaVe = raVm.GiaVe;
            ra.LoaiVeID = raVm.LoaiVeID;
            ra.MayTinhID_Vao = raVm.MayTinhID_Vao;
            ra.MayTinhID_Ra = raVm.MayTinhID_Ra;
            ra.BienSo = raVm.BienSo;
            ra.NhanVienID_Vao = raVm.NhanVienID_Vao;
            ra.NhanVienID_Ra = raVm.NhanVienID_Ra;
        }

        public static void UpdateHinhAnh(this HinhAnh hinhAnh, HinhAnhViewModel hinhAnhVm)
        {
            hinhAnh.MaHinhAnh = hinhAnhVm.MaHinhAnh;
            hinhAnh.HinhAnh_BYTE = hinhAnhVm.HinhAnh_BYTE;
            hinhAnh.CamNumber = hinhAnhVm.CamNumber;
        }

        public static void UpdateHinhAnhSave(this HinhAnhSave hinhAnhSave, HinhAnhSaveViewModel hinhAnhSaveVm)
        {
            hinhAnhSave.MaHinhAnh = hinhAnhSaveVm.MaHinhAnh;
            hinhAnhSave.HinhAnh_BYTE = hinhAnhSaveVm.HinhAnh_BYTE;
            hinhAnhSave.CamNumber = hinhAnhSaveVm.CamNumber;
        }

        public static void UpdateHinhAnhRa(this HinhAnhRa hinhAnhRa, HinhAnhRaViewModel hinhAnhRaVm)
        {
            hinhAnhRa.MaHinhAnh = hinhAnhRaVm.MaHinhAnh;
            hinhAnhRa.HinhAnh_BYTE = hinhAnhRaVm.HinhAnh_BYTE;
            hinhAnhRa.CamNumber = hinhAnhRaVm.CamNumber;
        }

        public static void UpdateXe(this Xe xe, XeViewModel xeVm)
        {
            xe.ID = xeVm.ID;
            xe.BienSo = xeVm.BienSo;
            xe.LoaiXeID = xeVm.LoaiXeID;
        }

        public static void UpdateLoaiXe(this LoaiXe loaiXe, LoaiXeViewModel loaiXeVm)
        {
            loaiXe.ID = loaiXeVm.ID;
            loaiXe.Ten = loaiXeVm.Ten;
            loaiXe.LoaiVeID = loaiXeVm.LoaiVeID;
            loaiXe.ThuTienTruoc = loaiXeVm.ThuTienTruoc;
            loaiXe.DangSuDung = loaiXeVm.DangSuDung;
        }

        public static void UpdateGiaHan(this GiaHan giaHan, GiaHanViewModel giaHanVm)
        {
            giaHan.ID = giaHanVm.ID;
            giaHan.VeThangID = giaHanVm.VeThangID;
            giaHan.MaThe = giaHanVm.MaThe;
            giaHan.BienSo = giaHanVm.BienSo;
            giaHan.HoTen = giaHanVm.HoTen;
            giaHan.NgayGiaHan = giaHanVm.NgayGiaHan;
            giaHan.GiaVe = giaHanVm.GiaVe;
            giaHan.MayTinhID = giaHanVm.MayTinhID;
            giaHan.NhanVienID = giaHanVm.NhanVienID;
        }

        public static void UpdateLoaiVe(this LoaiVe loaiVe, LoaiVeViewModel loaiVeVm)
        {
            loaiVe.ID = loaiVeVm.ID;
            loaiVe.Ten = loaiVeVm.Ten;
            loaiVe.Gia_1 = loaiVeVm.Gia_1;
            loaiVe.Gia_2 = loaiVeVm.Gia_2;
            loaiVe.Gia_3 = loaiVeVm.Gia_3;
            loaiVe.Gia_4 = loaiVeVm.Gia_4;
            loaiVe.Gia_5 = loaiVeVm.Gia_5;
            loaiVe.Gia_6 = loaiVeVm.Gia_6;
            loaiVe.Gia_7 = loaiVeVm.Gia_7;
            loaiVe.Gia_8 = loaiVeVm.Gia_8;
            loaiVe.Gio_1 = loaiVeVm.Gio_1;
            loaiVe.Gio_2 = loaiVeVm.Gio_2;
            loaiVe.Gio_3 = loaiVeVm.Gio_3;
            loaiVe.Gio_4 = loaiVeVm.Gio_4;
            loaiVe.Gio_5 = loaiVeVm.Gio_5;
            loaiVe.Gio_6 = loaiVeVm.Gio_6;
            loaiVe.Gio_7 = loaiVeVm.Gio_7;
            loaiVe.Gio_8 = loaiVeVm.Gio_8;
            loaiVe.Gio_9 = loaiVeVm.Gio_9;
            loaiVe.GioiHan_1 = loaiVeVm.GioiHan_1;
            loaiVe.GioiHan_2 = loaiVeVm.GioiHan_2;
            loaiVe.GioiHan_3 = loaiVeVm.GioiHan_3;
            loaiVe.GioiHan_4 = loaiVeVm.GioiHan_4;
            loaiVe.GioiHan_5 = loaiVeVm.GioiHan_5;
            loaiVe.GioiHan_6 = loaiVeVm.GioiHan_6;
            loaiVe.GioiHan_7 = loaiVeVm.GioiHan_7;
            loaiVe.GioiHan_8 = loaiVeVm.GioiHan_8;
            loaiVe.CongDon = loaiVeVm.CongDon;
            loaiVe.DangSuDung = loaiVeVm.DangSuDung;
        }

        public static void UpdateMayTinh(this MayTinh mayTinh, MayTinhViewModel mayTinhVm)
        {
            mayTinh.ID = mayTinhVm.ID;
            mayTinh.Ten = mayTinhVm.Ten;
            mayTinh.NgayTao = mayTinhVm.NgayTao;
        }

        public static void UpdateNhanVien(this NhanVien nhanVien, NhanVienViewModel nhanVienVm)
        {
            nhanVien.ID = nhanVienVm.ID;
            nhanVien.Ten = nhanVienVm.Ten;
            nhanVien.DiaChi = nhanVienVm.DiaChi;
            nhanVien.SoDienThoai = nhanVienVm.SoDienThoai;
            nhanVien.Luong = nhanVienVm.Luong;
            nhanVien.DanhMucID = nhanVienVm.DanhMucID;
            nhanVien.GhiChu = nhanVienVm.GhiChu;
            nhanVien.DangSuDung = nhanVienVm.DangSuDung;
        }

        public static void UpdateThe(this The the, TheViewModel theVm)
        {
            the.MaThe = theVm.MaThe;
            the.NgayTao = theVm.NgayTao;
            the.NgaySua = theVm.NgaySua;
            the.DangSuDung = theVm.DangSuDung;
        }

        public static void UpdateVeThang(this VeThang veThang, VeThangViewModel veThangVm)
        {
            veThang.ID = veThangVm.ID;
            veThang.MaThe = veThangVm.MaThe;
            veThang.BienSo = veThangVm.BienSo;
            veThang.GiaVe = veThangVm.GiaVe;
            veThang.NgayDangKy = veThangVm.NgayDangKy;
            veThang.NgayHetHan = veThangVm.NgayHetHan;
            veThang.LoaiXe = veThangVm.LoaiXe;
            veThang.HoTen = veThangVm.HoTen;
            veThang.DanhMucID = veThangVm.DanhMucID;
            veThang.GhiChu = veThangVm.GhiChu;
            veThang.XeMay = veThangVm.XeMay;
            veThang.ThuThe = veThangVm.ThuThe;
            veThang.DangSuDung = veThangVm.DangSuDung;
        }
    }
}