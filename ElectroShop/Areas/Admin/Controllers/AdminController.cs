using ElectroShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroShop.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
		private dbElectroShopDataContext db = new dbElectroShopDataContext("Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True");
		// GET: Admin/Admin
		public ActionResult Index()
        {
			// Check if the user is authenticated
			if (Session["Admin"] != null)
			{
				return View();
			}
			else
			{
				// Redirect to the login action if not authenticated
				return RedirectToAction("Login");
			}
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Login(FormCollection f)
		{
			//Gán các giá trị người dùng nhập liệu cho các biến
			var sTenDN = f["UserName"];
			var sMatKhau = f["Password"];
			//Gán giá trị cho đối tượng được tạo mới (ad)
			ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau
			== sMatKhau);
			if (ad != null)
			{
				Session["Admin"] = ad;
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
			}
			return View();
		}
		public ActionResult Logout()
		{
			Session["Admin"] = null;
			return RedirectToAction("Login", "Admin");
		}
	}

}