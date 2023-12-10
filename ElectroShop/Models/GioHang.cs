using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroShop.Models
{
	public class GioHang
	{
		dbElectroShopDataContext db = new dbElectroShopDataContext("Data Source=DESKTOP-33LF4EI\\SQLEXPRESS;Initial Catalog=ElectroShop;Integrated Security=True");
		public int iMaSP { get; set; }
		public string sTenSP { get; set; }
		public string sAnhBia { get; set; }
		public double dDonGia { get; set; }
		public int iSoLuong { get; set; }
		public double dThanhTien
		{
			get { return iSoLuong * dDonGia; }
		}

		public GioHang(int ms)
		{
			iMaSP = ms;
			SANPHAM s = db.SANPHAMs.Single(n => n.MaSP == iMaSP);
			sTenSP = s.TenSP;
			sAnhBia = s.AnhBia;
			dDonGia = double.Parse(s.GiaBan.ToString());
			iSoLuong = 1;
		}
	}
}