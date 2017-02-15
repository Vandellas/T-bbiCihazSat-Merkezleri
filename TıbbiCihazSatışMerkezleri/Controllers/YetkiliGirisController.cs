using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Models;
namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class YetkiliGirisController :Controller
    {
        // GET: YetkiliGiris
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            ContextEntitiess db = new ContextEntitiess();
            string username = form["kullanıcıadı"].ToString();
            string password = form["sifre"].ToString();
            var query = db.YetkiliBilgileri.Where(p => p.KullanıcıAdı == username && p.Şifre == password).ToList();
            if(query.Count>0)
            {
                HttpCookie User = new HttpCookie("User", "1");
                Response.Cookies.Add(User);
                HttpCookie UserName = new HttpCookie("UserName", username);
                Response.Cookies.Add(UserName);
                return RedirectToAction("FirmaGörüntüle", "Firmaincele");
            }
            else
                TempData["KayıtDurum"] = "Lütfen Bilgilerinizi Kontrol Ediniz";
            return View();
        }
        public ActionResult SignOut()
        {
            DateTime now = DateTime.UtcNow;
            string cookieKey = "User";
            var cookie = new HttpCookie(cookieKey, null)
            {
                Expires = now.AddDays(-1)
            };
            HttpContext.Response.Cookies.Set(cookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult YetkiVer()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Yetkiver(FormCollection form)
        {
            string userName = form["KullanıcıAdı"];
            string Password = form["Sifre"];
            ContextEntitiess context = new ContextEntitiess();
            YetkiliBilgileri yetkili = new YetkiliBilgileri();
            int count = context.YetkiliBilgileri.Where(p => p.KullanıcıAdı == userName.ToLower()).Select(p => p.id).FirstOrDefault();
            if (count == 0)
            {
                yetkili.KullanıcıAdı = userName;
                yetkili.Şifre = Password;
                context.YetkiliBilgileri.Add(yetkili);
                context.SaveChanges();
                TempData["YetkiKontrol"] = "Başarıyla Kaydedilmiştir";
            }
            else
            {
                TempData["YetkiKontrol"] = "Kullanıcı adı kullanılmakta";
            }
            return View();
        }
    }
}