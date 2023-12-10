using ElectroShop.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace ElectroShop.Controllers
{
    public class GioHangController : Controller
    {
		// GET: GioHang
		/*public ActionResult Index()
        {
            return View();
        }*/
		private dbElectroShopDataContext db = new dbElectroShopDataContext("Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True");

		public List<GioHang> LayGioHang()
		{
			List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
			if (lstGioHang == null)
			{
				lstGioHang = new List<GioHang>();
				Session["GioHang"] = lstGioHang;
			}
			return lstGioHang;
		}

		public ActionResult ThemGioHang(int ms, string url)
		{
			List<GioHang> lstGioHang = LayGioHang();
			GioHang sp = lstGioHang.Find(n => n.iMaSP == ms);
			if (sp == null)
			{
				sp = new GioHang(ms);
				lstGioHang.Add(sp);
			}
			else
			{
				sp.iSoLuong++;
			}
			return Redirect(url);
		}

		private int TongSoLuong()
		{
			int iTongSoLuong = 0;
			List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
			if (lstGioHang != null)
			{
				iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
			}
			return iTongSoLuong;
		}

		private double TongTien()
		{
			double dTongTien = 0;
			List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
			if (lstGioHang != null)
			{
				dTongTien = lstGioHang.Sum(n => n.dThanhTien);
			}
			return dTongTien;
		}

		public ActionResult GioHang()
		{
			List<GioHang> lstGioHang = LayGioHang();
			if (lstGioHang.Count == 0)
			{
				return RedirectToAction("Index", "ElectroShop");
			}
			ViewBag.TongSoLuong = TongSoLuong();
			ViewBag.TongTien = TongTien();
			return View(lstGioHang);
		}

		public ActionResult GioHangPartial()
		{
			ViewBag.TongSoLuong = TongSoLuong();
			ViewBag.TongTien = TongTien();
			return PartialView();
		}

		public ActionResult XoaSPKhoiGioHang(int iMaSP)
		{
			var gioHang = LayGioHang();
			var sanPham = gioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
			if (sanPham != null)
			{
				gioHang.Remove(sanPham);
				if (gioHang.Count == 0)
				{
					return RedirectToAction("Index", "ElectroShop");
				}
			}
			return RedirectToAction("GioHang");
		}
		public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
		{
			var gioHang = LayGioHang();
			var sanPham = gioHang.SingleOrDefault(n => n.iMaSP == iMaSP);
			if (sanPham != null)
			{
				sanPham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
			}
			return RedirectToAction("GioHang");
		}
		public ActionResult XoaGioHang()
		{
			var gioHang = LayGioHang();
			gioHang.Clear();
			return RedirectToAction("Index", "ElectroShop");
		}

		[HttpGet]
		public ActionResult DatHang()
		{
			
			if (Session["GioHang"] == null)
			{
				return RedirectToAction("Index", "ElectroShop");
			}
			List<GioHang> lstGioHang = LayGioHang();
			ViewBag.TongSoLuong = TongSoLuong();
			ViewBag.TongTien = TongTien();
			return View(lstGioHang);
		}
		[HttpPost]
		public ActionResult DatHang(FormCollection f)
		{
			DONDATHANG ddh = new DONDATHANG();
			List<GioHang> lstGioHang = LayGioHang();

			var sHoTen = f["HoTen"];
			var sEmail = f["Email"];
			var sDienThoai = f["DienThoai"];
			var sDiaChi = f["DiaChi"];

			if (String.IsNullOrEmpty(sHoTen))
			{
				TempData["HoTen"] = sHoTen;
				ViewData["err1"] = "Họ tên không được rỗng";
			}
			else if (String.IsNullOrEmpty(sEmail))
			{
				TempData["Email"] = sEmail;
				ViewData["err2"] = "Email không được rỗng";
			}
			else if (String.IsNullOrEmpty(sDienThoai))
			{
				ViewData["err3"] = "Số điện thoại không được rỗng";
			}
			else if (String.IsNullOrEmpty(sDiaChi))
			{
				ViewData["err4"] = "Địa chỉ không được rỗng";
			}
			else
			{
				// Điền thông tin khách hàng vào đơn đặt hàng
				ddh.HoTen = sHoTen;
				ddh.Email = sEmail;

				// Handle phone number conversion
				if (int.TryParse(sDienThoai, out int phoneNumber))
				{
					ddh.DienThoai = phoneNumber;
				}
				else
				{
					// Handle invalid phone number, set default or take appropriate action
					ddh.DienThoai = 0; // You may want to set a default value or handle it differently
				}

				ddh.DiaChi = sDiaChi;

				// Các thông tin khác không thay đổi
				ddh.NgayDat = DateTime.Now;

				ddh.TinhTrangGiaoHang = 1;
				ddh.DaThanhToan = false;
				ddh.ThanhTien = (decimal?)TongTien();

				db.DONDATHANGs.InsertOnSubmit(ddh);
				db.SubmitChanges();

				// Thêm chi tiết đơn hàng
				foreach (var item in lstGioHang)
				{
					CHITIETDATHANG ctdh = new CHITIETDATHANG();
					ctdh.MaDonHang = ddh.MaDonHang;
					ctdh.MaSP = item.iMaSP;
					ctdh.SoLuong = item.iSoLuong;
					ctdh.DonGia = (decimal)item.dDonGia;
					db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
				}

				db.SubmitChanges();
				SendMail(ddh, ddh.Email, ddh.HoTen);
				Session["GioHang"] = null;

				return RedirectToAction("XacNhanDonHang", "GioHang");
			}
			return this.DatHang();
			
		}

		private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
		private static string Email = ConfigurationManager.AppSettings["Email"];

		public static bool SendMail(DONDATHANG ddh, string toMail, string name)
		{
			bool rs = false;
			try
			{
				System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
				var smtp = new System.Net.Mail.SmtpClient();
				{
					smtp.Host = "smtp.gmail.com";
					smtp.Port = 587;
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential()
					{
						UserName = Email,
						Password = password
					};
				}
				MailAddress fromAddress = new MailAddress(Email);
				message.From = fromAddress;
				message.To.Add(toMail);
				message.Subject = $"Xác nhận đơn hàng #{ddh.MaDonHang}";
				message.IsBodyHtml = true;
				message.Body = $"Cảm ơn bạn đã đặt hàng. Mã đơn hàng của bạn là {ddh.MaDonHang}.";
				smtp.Send(message);
				rs = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				rs = false;
			}
			return rs;
		}

		public ActionResult XacNhanDonHang()
		{
			return View();
		}
	}
}