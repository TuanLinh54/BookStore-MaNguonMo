using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

using PagedList;
using PagedList.Mvc;
namespace MvcBookStore.Controllers
{
    public class ShopController : Controller
    {
        dbBookstoreDataContext data = new dbBookstoreDataContext();
        // GET: Shop
        private List<SACH> Laysachmoi(int count)
        {
            //Sap xep giam dan theo Ngaycapnhat, lay count dong dau
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult IndexShop(int? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 6;
            //Tao bien so trang
            int pageNum = (page ?? 1);

            //Lay  quyen sach moi nhat
            //var sachmoi = from SACH in data.SACHes select SACH;
            var sachmoi = Laysachmoi(18);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
    }
}