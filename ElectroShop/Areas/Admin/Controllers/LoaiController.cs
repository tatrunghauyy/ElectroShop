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
    public class LoaiController : Controller
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
			return View(db.LOAISPs.ToList().OrderBy(n => n.MaLoai).ToPagedList(iPageNum, iPageSize));
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
		public ActionResult Create(LOAISP loai, FormCollection f, HttpPostedFileBase fFileUpLoad)
		{
			ViewBag.MaCD = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");

			if (ModelState.IsValid)
			{
				// Kiểm tra xem TenChuDe đã tồn tại trong cơ sở dữ liệu hay chưa
				var existingChuDe = db.LOAISPs.FirstOrDefault(c => c.TenLoai == loai.TenLoai);

				if (existingChuDe != null)
				{
					ModelState.AddModelError("TenLoai", "Tên loại đã tồn tại");
					return View(loai);
				}
				else
				{
					// Nếu TenChuDe chưa tồn tại, thêm mới
					db.LOAISPs.InsertOnSubmit(loai);
					db.SubmitChanges();
					return RedirectToAction("Index");
				}
			}
			return View(loai);
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var loai = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);
			if (loai == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(loai);
		}
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteComfirm(int id, FormCollection f)
		{
			var loai = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);

			if (loai == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			var ctdh = db.SANPHAMs.Where(ct => ct.MaLoai == id);
			if (ctdh.Count() > 0)
			{
				ViewBag.ThongBao = "Đang có sản phẩm thuộc loại này!! <br>" + " Nếu muốn xóa thì phải xóa hết các sản phẩm thuộc loại này";
				return View(loai);
			}
			db.LOAISPs.DeleteOnSubmit(loai);
			db.SubmitChanges();
			return RedirectToAction("Index");
		}
	}
}