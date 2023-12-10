using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectroShop.Models;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.Drawing.Printing;


namespace ElectroShop.Controllers
{
    public class ElectroShopController : Controller
    {
		private string connection;
		private dbElectroShopDataContext data;

		public ElectroShopController()
		{
			// Khởi tạo chuỗi kết nối
			connection = "Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True";
			data = new dbElectroShopDataContext(connection);
		}

		private List<SANPHAM> LaySanPham(int count)
		{
			return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
		}
		// GET: ElectroShop
		public ActionResult Index(int? page)
        {
			int pageSize = 6;
			int pageNumber = (page ?? 1);

			var listSanPham = LaySanPham(500);
			return View(listSanPham.ToPagedList(pageNumber, pageSize));
		}
		public ActionResult SanPhamBanNhieuPartial()
		{
			var listSanPhamBanNhieu = data.SANPHAMs.OrderByDescending(a => a.SoLuongBan).Take(6).ToList();

			return PartialView("SanPhamBanNhieuPartial", listSanPhamBanNhieu);
		}
		public ActionResult SapXep()
		{
			return PartialView("SapXepPartial");
		}
		private List<SANPHAM> LaySanPhamTheoGiaTangDan(int count)
		{
			return data.SANPHAMs.OrderBy(a => a.GiaBan).Take(count).ToList();
		}
		public ActionResult SanPhamTangDan(int? page)
		{
			int pageSize = 6;
			int pageNumber = (page ?? 1);

			var listSanPhamTangDan = LaySanPhamTheoGiaTangDan(500);

			return View(listSanPhamTangDan.ToPagedList(pageNumber, pageSize));
		}
		private List<SANPHAM> LaySanPhamTheoGiaGiamDan(int count)
		{
			return data.SANPHAMs.OrderByDescending(a => a.GiaBan).Take(count).ToList();
		}
		public ActionResult SanPhamGiamDan(int? page)
		{
			int pageSize = 6;
			int pageNumber = (page ?? 1);

			var listSanPhamGiamDan = LaySanPhamTheoGiaGiamDan(500);

			return View(listSanPhamGiamDan.ToPagedList(pageNumber, pageSize));
		}
		public ActionResult LoaiSPPartial()
		{
			var listloaiSP = from lsp in data.LOAISPs select lsp;
			return PartialView(listloaiSP);
		}
		private string LayLoaiSP(int id)
		{
			var chuDe = data.LOAISPs.SingleOrDefault(cd => cd.MaLoai == id);

			if (chuDe != null)
			{
				return chuDe.TenLoai;
			}

			return "Không tìm thấy chủ đề";
		}
		public ActionResult SanPhamTheoLoai(int id, int? page)
		{
			int pageSize = 3;
			int pageNumber = page ?? 1;

			var sp = data.SANPHAMs.Where(s => s.MaLoai == id)
								  .OrderByDescending(s => s.NgayCapNhat)
								  .ToPagedList(pageNumber, pageSize);

			ViewBag.Id = id;
			ViewBag.ten = LayLoaiSP(id);

			return View(sp);
		}
		public ActionResult HangPartial()
		{
			var listhang = from h in data.HANGs select h;
			return PartialView(listhang);
		}
		private string LayHang(int id)
		{
			var chuDe = data.HANGs.SingleOrDefault(cd => cd.MaHang == id);

			if (chuDe != null)
			{
				return chuDe.TenHang;
			}

			return "Không tìm thấy chủ đề";
		}
		public ActionResult SanPhamTheoHang(int id, int? page)
		{
			int pageSize = 3;
			int pageNumber = page ?? 1;

			var sp = data.SANPHAMs.Where(s => s.MaHang == id)
								  .OrderByDescending(s => s.NgayCapNhat)
								  .ToPagedList(pageNumber, pageSize);

			ViewBag.Id = id;
			ViewBag.ten = LayHang(id);

			return View(sp);
		}
		public ActionResult ChiTietSP(int id)
		{
			var sach = from s in data.SANPHAMs
					   where s.MaSP == id
					   select s;
			return View(sach.Single());
		}
		public ActionResult LoginLogout()
		{
			return PartialView("LoginLogoutPartial");
		}
		public ActionResult TimKiemSanPham(string keyword, int? page)
		{
			// Check if the keyword is provided
			if (string.IsNullOrEmpty(keyword))
			{
				// Handle the case where no keyword is provided (you can redirect to the home page or show an error message)
				return RedirectToAction("Index");
			}

			int pageSize = 3;
			int pageNumber = page ?? 1;

			// Perform the search based on the product name
			var searchResults = data.SANPHAMs
									.Where(s => s.TenSP.Contains(keyword))
									.OrderByDescending(s => s.NgayCapNhat)
									.ToPagedList(pageNumber, pageSize);

			ViewBag.Keyword = keyword; // Pass the keyword to the view

			return View(searchResults);
		}
	}
}