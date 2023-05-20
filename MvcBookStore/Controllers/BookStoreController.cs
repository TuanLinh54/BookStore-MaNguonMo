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
    public class BookStoreController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu dbBookstore
        dbBookstoreDataContext data = new dbBookstoreDataContext();

        private List<SACH> Laysachmoi(int count)
        {
            //Sap xep giam dan theo Ngaycapnhat, lay count dong dau
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int ? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 4;
            //Tao bien so trang
            int pageNum = (page ?? 1);

            //Lay  quyen sach moi nhat
            //var sachmoi = from SACH in data.SACHes select SACH;
            var sachmoi = Laysachmoi(16);
            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }

        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var NXB = from NHAXUATBAN in data.NHAXUATBANs select NHAXUATBAN;
            return PartialView(NXB);
        }

        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD==id select s;
            return PartialView(sach);
        }

        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return PartialView(sach);
        }

        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return PartialView(sach.Single());
        }

        public ActionResult Search(FormCollection collection)
        {
            var search = collection["search"].ToLower();
            var sachmoi = from SACH in data.SACHes where SACH.Tensach.ToLower().Contains(search) select SACH;
            ViewBag.keyWord = collection["search"];
            return View(sachmoi);
        }
    }
}