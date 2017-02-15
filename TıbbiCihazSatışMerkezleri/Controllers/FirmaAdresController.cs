using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Control;
using TıbbiCihazSatışMerkezleri.Models;

namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class FirmaAdresController : BaseControl
    {
        // GET: FirmaAdres
        public ActionResult Index()
        {
            ContextEntitiess myentity = new ContextEntitiess();
            ViewBag.illist = new SelectList(myentity.iller, "sehirid", "sehir");
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            FirmaBilgileridb adres = new FirmaBilgileridb();
            ContextEntitiess myentity = new ContextEntitiess();
            ViewBag.illist = new SelectList(myentity.iller, "sehirid", "sehir");
            int ilid = Int32.Parse(form["il"]);
            int ilceid = Int32.Parse(form["ilce"]);
            string il = myentity.iller.Where(q => q.sehirid == ilid).Select(q => q.sehir).FirstOrDefault();
            string ilce = myentity.ilceler.Where(q => q.id == ilceid).Select(q => q.ilce).FirstOrDefault();
            adres.ilce = ilce;
            adres.il = il; ;
            adres.Mahalle = form["Mahalle"];
            adres.FaxNo = form["FaxNo"];
            adres.FirmaEposta = form["Eposta"];
            adres.Cadde = form["Cadde"];
            adres.DışKapıNo = form["DışNo"];
            adres.İçKapıNo = form["İçNo"];
            adres.TelNo = form["TelNo"];
            adres.TelNo2 = form["TelNo2"];
            adres.VergiDairesi = form["VergiDaire"];
            adres.Sokak = form["Sokak"];
            adres.WebSite = form["WebSite"];
            adres.PostaKodu = form["PostaNo"];
            TempData["adres"] = adres;
            return RedirectToAction("Index","ÇalışanBilgileri");
        }
        public JsonResult Getİlce(int id)
        {
            var db = new ContextEntitiess();//Veri tabanımızı çağırdık
            var query = db.ilceler.OrderBy(x => x.ilce).Where(x => x.sehir == id).Select(x => new//Veritabanından id'ye göre listeleme işlemini yaptık.
            {
                Value = x.id,//Value isminde dinamik alan oluşturduk
                Text = x.ilce//Text isminde dinamik alan oluşturduk
            }).ToList();
            if (id == 0)//gelen id null ise geriye 0 döndürdük
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(query, JsonRequestBehavior.AllowGet);//id null değilse veritabanından çektiğimiz kayıtları döndürdük.
        }
      
        public ActionResult temp()
        {
            return View();
        }
    }
}