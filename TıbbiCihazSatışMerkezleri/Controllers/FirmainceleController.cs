using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Control;
using TıbbiCihazSatışMerkezleri.Models;
namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class FirmainceleController : AdminControl
    {
        ContextEntitiess context = new ContextEntitiess();
        // GET: Firmaincele
        public ActionResult FirmaGörüntüle()
        {

            List<FirmaSahibidb> firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
            return View(firmasahibi);
        }
        [HttpPost]
        public ActionResult FirmaGörüntüle(FormCollection form)
        {
            if (form["görüntüle"] == "select")
            {
              
                List<object> myModel = new List<object>();
                myModel.Clear();
                string sorgu = form["sorgula"];
                string secenek = form["secenek"];
                List<FirmaSahibidb> firmasahibi;
                if (secenek == "0")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.VergiNo.Contains(sorgu) && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                else if (secenek == "1")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaAdı.Contains(sorgu) && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                else if (secenek == "2")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.AdıSoyadı.Contains(sorgu) && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                else if (secenek == "3")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.il.Contains(sorgu) && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                else if (secenek == "4")
                {
                    if ("merkez".Contains(sorgu.ToLower()))
                        firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.Type == "0" && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                    else if ("şube".Contains(sorgu.ToLower()))
                        firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.Type == "1" && p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();
                    else
                        firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.Type == "2").ToList();
                }
                else if (secenek == "6")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 2).ToList();
                else if(secenek=="7")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 1).ToList();
                else if (secenek == "8")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 1||p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum==2).ToList();
                else if (secenek == "9")
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 3).ToList();
                else
                    firmasahibi = context.FirmaSahibidb.Where(p => p.FirmaBilgileridb.FirmaDurum.FirstOrDefault().Durum == 0).ToList();

                return View(firmasahibi);
            }
            else
            {
                Session["görüntüle"] = form["görüntüle"].ToString();
                return RedirectToAction("Firmaincele", "Firmaincele");
            }
        }
        public ActionResult Firmaincele()
        {
          
            List<object> myModel = new List<object>();
            int id;
            try
            {
                 id = Int32.Parse(Session["görüntüle"].ToString());
            }
            catch (Exception)
            {

                id = 0;
            }
            var sahip = context.FirmaSahibidb.Where(p => p.Firmaid == id).ToList();
            return View(sahip);
        }
        [HttpPost]
        public ActionResult Firmaincele(FormCollection form)
        {
            int kontrolid = Int32.Parse(form["kontrolid"]);
            if (form["Tamam"]=="Kabul")
            {
                FirmaDurum durum = new FirmaDurum();
                durum = context.FirmaDurum.Find(kontrolid);
                durum.Durum = 1;
                durum.Tarih = DateTime.Now;
                durum.RedSebebi = "";
                durum.YapanKişi = Request.Cookies.Get("UserName").Value;
                context.Entry(durum).State = EntityState.Modified;
                context.SaveChanges();
                TempData["Onaylama"] = "İşleminiz Gerçekleşmiştir";
            }
            else
            {
                FirmaDurum durum = new FirmaDurum();
                durum = context.FirmaDurum.Find(kontrolid);
                durum.Durum = 2;
                durum.Tarih = DateTime.Now;
                durum.YapanKişi = Request.Cookies.Get("UserName").Value;
                durum.RedSebebi = form["hide"];
                context.Entry(durum).State = EntityState.Modified;
                context.SaveChanges();
                TempData["Onaylama"] = "İşleminiz Gerçekleşmiştir";
            }
            int id;
            try
            {
                id = Int32.Parse(Session["görüntüle"].ToString());
            }
            catch (Exception)
            {

                id = 0;
            }
            var sahip = context.FirmaSahibidb.Where(p => p.Firmaid == id).ToList();
            return View(sahip);
        }

    }
}