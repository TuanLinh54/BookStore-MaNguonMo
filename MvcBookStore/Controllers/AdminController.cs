using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;
using System.IO;

using PagedList;
using PagedList.Mvc;
namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu dbBookstore
        dbBookstoreDataContext data = new dbBookstoreDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sach(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(data.SACHes.ToList() );
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập Tên tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)

                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbap = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad.Hoten;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không đúng !";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileupload )
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using System.IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //Kiem tra hinh anh ton tai chua?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    //Luu vao CSDL
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
           
        }

        //Hiển thị sản phẩm
        public ActionResult Chitietsach(int id)
        {
            //Lay ra doi tuong sach theo ma
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //Lay ra doi tuong can xoa theo ma
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SACHes.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }

        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Lay ra doi tuong sach theo ma
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload, FormCollection f)
        {
            // Đưa dữ liệu vào DropdownList
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            // Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
                return View();
            }
            // Thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    // Lưu tên file, lưu ý bổ sung thư viện System.IO
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    // Kiểm tra hình ảnh tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!";
                    }
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    int masach = Int32.Parse(f.Get("Masach"));
                    sach = data.SACHes.SingleOrDefault(n => n.Masach == masach);
                    sach.Anhbia = fileName;
                    sach.Tensach = f.Get("Tensach");
                    sach.Giaban = Decimal.Parse(f.Get("Giaban"));
                    sach.Mota = f.Get("Mota");
                    sach.Ngaycapnhat = DateTime.Parse(f.Get("Ngaycapnhat"));
                    sach.Soluongton = Int32.Parse(f.Get("Soluongton"));
                    // Lưu vào CSDL
                    UpdateModel(sach);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Sach");
        }
    }
}