using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dangnhap_1.Models;

namespace Dangnhap_1.Controllers
{

    public class UserController : Controller
    {
        // GET: User
        MovieWebDataContext db = new MovieWebDataContext();
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, NguoiDung nd)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection["hoten"];
            var gioitinh = collection["GioiTinh"];
            var sdt = collection["sdt"];
            var tendn = collection["tdn"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var email = collection["email"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(gioitinh))
            {
                ViewData["Loi2"] = "Phải nhập giới tính";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi3"] = "Email không được bỏ trống";
            }
            else if (!IsValidEmail(email))
            {
                ViewData["Loi3"] = "Hãy nhập một email";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi4"] = "Số điện thoại không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi5"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi6"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi7"] = "Phải nhập lại mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                nd.HoTen = hoten;
                nd.GioiTinh = gioitinh;
                nd.Email = email;
                nd.SDT = sdt;
                nd.TaiKhoan = tendn;
                nd.Email = email;
                nd.MatKhau = matkhau;
                db.NguoiDungs.InsertOnSubmit(nd);
                db.SubmitChanges();
                return RedirectToAction("Login", "User");
            }
            return this.Register();
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["Tendn"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống!";
            }
            else if (!String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống!";
            }
            else
            {
                NguoiDung N = db.NguoiDungs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (N != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!";
                    Session["TaiKhoan"] = N;
                }
                else
                    ViewBag.Thongbao = "Đăng nhập thất bại. Vui lòng thử lại!";
            }
            return View();
        }
    }
}