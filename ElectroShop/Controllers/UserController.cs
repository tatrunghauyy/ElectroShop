﻿using ElectroShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroShop.Controllers
{
    public class UserController : Controller
    {
		private String connection;
		private dbElectroShopDataContext db;

		public UserController()
		{
			// Khởi tạo chuỗi kết nối
			connection = "Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True";
			db = new dbElectroShopDataContext(connection);
		}

		[HttpGet]
		public ActionResult Dangky()
        {
            return View();
        }
		/*[HttpPost]
		public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
		{
			var sHoTen = collection["HoTen"];
			var sTenDN = collection["TenDN"];
			var sMatKhau = collection["MatKhau"];
			var sMatKhauNhapLai = collection["MatKhauNL"];
			var sDiaChi = collection["DiaChi"];
			var sEmail = collection["Email"];
			var sDienThoai = collection["DienThoai"];
			var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
			if (String.IsNullOrEmpty(sHoTen))
			{
				TempData["HoTen"] = sHoTen;
				ViewData["err1"] = "Họ tên không được rỗng";
			}
			else if (String.IsNullOrEmpty(sTenDN))
			{
				TempData["TenDN"] = sTenDN;
				ViewData["err2"] = "Tên đăng nhập không được rỗng";
			}
			else if (String.IsNullOrEmpty(sMatKhau))
			{
				TempData["MatKhau"] = sMatKhau;
				ViewData["err3"] = "Phải nhập mật khẩu";
			}
			else if (String.IsNullOrEmpty(sMatKhauNhapLai))
			{
				TempData["MatKhauNL"] = sMatKhauNhapLai;
				ViewData["err4"] = "Phải nhập lại mật khẩu";
			}
			else if (sMatKhau != sMatKhauNhapLai)
			{
				ViewData["err4"] = "Mật khẩu nhập lại không khớp";
			}
			else if (String.IsNullOrEmpty(sEmail))
			{
				TempData["Email"] = sEmail;
				ViewData["err5"] = "Email không được rỗng";
			}
			else if (String.IsNullOrEmpty(sDienThoai))
			{
				ViewData["err6"] = "Số điện thoại không được rỗng";
			}
			else if (String.IsNullOrEmpty(dNgaySinh))
			{
				ViewData["err7"] = "Ngày sinh không được rỗng";
			}
			else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
			{
				ViewBag.ThongBao = "Email đã được sử dụng";
			}
			else
			{
				kh.HoTen = sHoTen;
				kh.MatKhau = sMatKhau;
				kh.Email = sEmail;
				kh.DiaChi = sDiaChi;
				kh.DienThoai = sDienThoai;
				kh.NgaySinh = DateTime.Parse(dNgaySinh);
				db.KHACHHANGs.InsertOnSubmit(kh);
				db.SubmitChanges();
				return RedirectToAction("DangNhap");
			}
			return this.Dangky();
		}

		[HttpGet]
		public ActionResult DangNhap()
		{
			return View();
		}

		[HttpPost]
		public ActionResult DangNhap(FormCollection collection)
		{
			var sTenDN = collection["Email"];
			var sMatKhau = collection["MatKhau"];
			if (String.IsNullOrEmpty(sTenDN))
			{
				ViewData["Err1"] = "Bạn chưa nhập email";
			}
			else if (String.IsNullOrEmpty(sMatKhau))
			{
				ViewData["Err2"] = "Phải nhập mật khẩu";
			}
			else
			{
				KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Email == sTenDN && n.MatKhau == sMatKhau);
				if (kh != null)
				{
					ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
					Session["Email"] = kh;
					return RedirectToAction("DatHang", "GioHang");
				}
				else
				{
					ViewBag.ThongBao = "Email hoặc mật khẩu không đúng";
				}
			}
			return View();
		}

		public ActionResult DangXuat()
		{
			// Xóa tài khoản khỏi Session hoặc thực hiện bất kỳ xử lý đăng xuất nào cần thiết.
			Session["TaiKhoan"] = null; // Xóa tài khoản khỏi Session

			// Chuyển hướng đến trang đăng nhập hoặc trang chính, hoặc trang khác tùy ý.
			return RedirectToAction("DangNhap"); // Chuyển hướng đến trang đăng nhập sau khi đăng xuất.
		}
		*/
	}
}