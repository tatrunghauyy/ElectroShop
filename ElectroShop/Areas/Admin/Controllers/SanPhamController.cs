using System.Web;
using System.Web.Mvc;
using ElectroShop.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Linq;
using System;

namespace ElectroShop.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
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
			return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(iPageNum, iPageSize));
		}
		[HttpGet]
		public ActionResult Create()
		{
			ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
			ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
			return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(SANPHAM sp, FormCollection f, HttpPostedFileBase fFileUpLoad)
		{
			//Đưa dữ liệu vào DropDown
			ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
			ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
			if (fFileUpLoad == null)
			{
				//Nội dung thông báo yêu cầu chọn ảnh bìa
				ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
				//Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang
				ViewBag.TenSach = f["sTenSP"];
				ViewBag.MoTa = f["sMoTa"];
				ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
				ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
				ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", int.Parse(f["MaHang"]));
				ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", int.Parse(f["MaLoai"]));
				return View();
			}
			else
			{
				if (ModelState.IsValid)
				{
					//Lấy tên file (Khai báo thư viện: System.IO)
					var sFileName = Path.GetFileName(fFileUpLoad.FileName);
					//Lấy đường dẫn lưu file
					var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
					//Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
					if (!System.IO.File.Exists(path))
					{
						fFileUpLoad.SaveAs(path);
					}
					//Lưu Sach vào CSDL
					sp.TenSP = f["sTenSP"];
					sp.MoTa = f["sMota"];
					sp.AnhBia = sFileName;
					sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
					sp.SoLuongBan = int.Parse(f["iSoLuong"]);
					sp.GiaBan = decimal.Parse(f["mGiaBan"]);
					sp.MaHang = int.Parse(f["MaHang"]);
					sp.MaLoai = int.Parse(f["MaLoai"]);
					db.SANPHAMs.InsertOnSubmit(sp);
					db.SubmitChanges();
					return RedirectToAction("Index");
				}
				return View();
			}
		}
		public ActionResult Details(int id)
		{
			var sach = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
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
			var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
			if (sp == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(sp);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteComfirm(int id, FormCollection f)
		{
			var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);

			if (sp == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSP == id);
			if (ctdh.Count() > 0)
			{
				//Nội dung sẽ hiển thị khi sách cần xóa đã có trong table ChiTietDonHang
				ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
				return View(sp);
			}
			//Xóa sách
			db.SANPHAMs.DeleteOnSubmit(sp);
			db.SubmitChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{
			var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
			if (sp == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
			ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", sp.MaHang);
			return View(sp);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
		{
			if (f == null)
			{
				// Handle the case where FormCollection is null
				return RedirectToAction("Index");
			}

			var iMaSP = f["iMaSP"];
			if (iMaSP == null)
			{
				// Handle the case where iMaSP is null
				return RedirectToAction("Index");
			}

			var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == int.Parse(iMaSP));
			if (sp == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
			ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", sp.MaHang);

			if (ModelState.IsValid)
			{
				if (fFileUpload != null && !string.IsNullOrEmpty(fFileUpload.FileName))
				{
					var sFileName = Path.GetFileName(fFileUpload.FileName);
					var path = Path.Combine(Server.MapPath("~/Images"), sFileName);

					if (!System.IO.File.Exists(path))
					{
						fFileUpload.SaveAs(path);
					}
					sp.AnhBia = sFileName;
				}

				sp.TenSP = f["sTenSP"];
				sp.MoTa = f["sMoTa"];
				sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
				sp.SoLuongBan = int.Parse(f["iSoLuong"]);
				sp.GiaBan = decimal.Parse(f["mGiaBan"]);
				sp.MaLoai = int.Parse(f["MaLoai"]);
				sp.MaHang = int.Parse(f["MaHang"]);

				db.SubmitChanges();
				return RedirectToAction("Index");
			}

			return View(sp);
		}

	}
}