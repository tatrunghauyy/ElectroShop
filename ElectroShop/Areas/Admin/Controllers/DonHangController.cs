using System.Web;
using System.Web.Mvc;
using ElectroShop.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Linq;
using System;
using System.Globalization;

namespace ElectroShop.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
		private dbElectroShopDataContext db = new dbElectroShopDataContext("Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True");
		// GET: Admin/Sach
		public ActionResult Index(int? page)
		{
			if (Session["Admin"] == null)
			{
				return RedirectToAction("Login", "Admin");
			}
			int iPageNum = (page ?? 1);
			int iPageSize = 7;
			return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
		}
		public ActionResult Details(int id)
		{
			var sach = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
			if (sach == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(sach);
		}
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var donHang = db.DONDATHANGs.SingleOrDefault(dh => dh.MaDonHang == id);

			if (donHang == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(donHang);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirm(int id)
		{
			var donHang = db.DONDATHANGs.SingleOrDefault(dh => dh.MaDonHang == id);

			if (donHang == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			// Retrieve all related CHITIETDATHANG records
			var chiTietDonHangs = db.CHITIETDATHANGs.Where(ct => ct.MaDonHang == id);

			// Delete each related CHITIETDATHANG record
			foreach (var chiTiet in chiTietDonHangs)
			{
				db.CHITIETDATHANGs.DeleteOnSubmit(chiTiet);
			}

			// Delete the DONDATHANG record
			db.DONDATHANGs.DeleteOnSubmit(donHang);

			db.SubmitChanges();

			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Edit(int id)
		{
			var donHang = db.DONDATHANGs.SingleOrDefault(dh => dh.MaDonHang == id);
			if (donHang == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(donHang);
		}

		[HttpPost]
		public ActionResult Edit(int id, FormCollection form)
		{
			var donHang = db.DONDATHANGs.SingleOrDefault(dh => dh.MaDonHang == id);
			if (donHang == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			if (ModelState.IsValid)
			{
				// Update order information
				donHang.DaThanhToan = bool.Parse(form["DaThanhToan"] == "true" ? "True" : "False");
				donHang.TinhTrangGiaoHang = int.Parse(form["TinhTrangGiaoHang"]);
				donHang.NgayGiao = DateTime.Parse(form["NgayGiao"]);
				// Save changes to the database
				db.SubmitChanges();

				// Redirect back to the order list or any other appropriate action
				return RedirectToAction("Index");
			}

			return View(donHang);
		}



	}
}