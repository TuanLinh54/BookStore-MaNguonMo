using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class Giohang
    {
        //Tạo đối tượng data chứa dữ liệu từ model dbBookstore đã tạo.
        dbBookstoreDataContext data = new dbBookstoreDataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public
        string sAnhbia
                { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        //Khởi tạo giỏ hàng theo Masach được truyền vào với số lượng mặc định là 1
        public Giohang(int Masach)
            {
                iMasach = Masach;
                SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
                sTensach = sach.Tensach;
                sAnhbia = sach.Anhbia;
                dDongia = double.Parse(sach.Giaban.ToString());
                iSoluong = 1;
            }
     }
}