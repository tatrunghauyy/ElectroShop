using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectroShop.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace ElectroShop.Areas.Admin.Controllers
{
    public class HangController : Controller
    {
		private dbElectroShopDataContext db = new dbElectroShopDataContext("Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True");
		// GET: Admin/ChuDe
		public ActionResult Index(int? page)
		{
			if (Session["Admin"] == null)
			{
				return RedirectToAction("Login", "Admin");
			}
			int iPageNum = (page ?? 1);
			int iPageSize = 7;
			return View(db.HANGs.ToList().OrderBy(n => n.MaHang).ToPagedList(iPageNum, iPageSize));
		}
		[HttpGet]
		public ActionResult Create()
		{
			if (Session["Admin"] == null)
			{
				return RedirectToAction("Login", "Admin");
			}
			ViewBag.MaCD = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
			return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(HANG hang, FormCollection f, HttpPostedFileBase fFileUpLoad)
		{
			ViewBag.MaCD = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");

			if (ModelState.IsValid)
			{
				// Kiểm tra xem TenChuDe đã tồn tại trong cơ sở dữ liệu hay chưa
				var existingChuDe = db.HANGs.FirstOrDefault(c => c.TenHang == hang.TenHang);

				if (existingChuDe != null)
				{
					ModelState.AddModelError("TenLoai", "Tên loại đã tồn tại");
					return View(hang);
				}
				else
				{
					// Nếu TenChuDe chưa tồn tại, thêm mới
					db.HANGs.InsertOnSubmit(hang);
					db.SubmitChanges();
					return RedirectToAction("Index");
				}
			}
			return View(hang);
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var hang = db.HANGs.SingleOrDefault(n => n.MaHang == id);
			if (hang == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(hang);
		}
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteComfirm(int id, FormCollection f)
		{
			var hang = db.HANGs.SingleOrDefault(n => n.MaHang == id);

			if (hang == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			var ctdh = db.SANPHAMs.Where(ct => ct.MaHang == id);
			if (ctdh.Count() > 0)
			{
				ViewBag.ThongBao = "Đang có sản phẩm thuộc hãng này!! <br>" + " Nếu muốn xóa thì phải xóa hết các sản phẩm thuộc hãng";
				return View(hang);
			}
			db.HANGs.DeleteOnSubmit(hang);
			db.SubmitChanges();
			return RedirectToAction("Index");
		}
	}
}