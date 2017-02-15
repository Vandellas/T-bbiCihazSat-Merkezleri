using RazorPDF;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Control;
using TıbbiCihazSatışMerkezleri.Models;
namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class TaahhütnameController : BaseControl
    {
        ContextEntitiess db = new ContextEntitiess();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {

                try
                {
                    string vergiNo = TempData["vergi"].ToString();
                    string firmayapısı = TempData["firmayapısı"].ToString();
                    int id = addfirmabilgileri(vergiNo, firmayapısı);
                    addFirmaSahibi(id);
                    addMesulMüdür(id);
                    addFirmaDosyaları(id);
                    addDurum(id);
                    addPersonel(id);
                    int Kontrolid = db.FirmaDurum.Where(q => q.Firmaid == id).Select(q => q.Kontrolid).First();
                    TempData["Success"] = "Kaydınız Başarılı Şekilde Kaydedilmiştir Kontrol etmel için Kontrol Numaranız:" + Kontrolid;
                    TempData["id"] = id;

                   return RedirectToAction("Pdf", "Taahhütname", new { FileUploadMsg = "Kaydınız Başarılı Şekilde Kaydedilmiştir Kontrol etmel için Kontrol Numaranız:" + Kontrolid });
               
              }
                catch (Exception)
                {
                TempData["Success"] = "Kaydız Gerçekleştirilemedi lütfen bilgilerinizi kontrol ediniz..";
                return View();
                }


               
        }

        public int addfirmabilgileri(string vergiNo,string firmayapısı)
        {
            FirmaBilgileridb adres = TempData["adres"] as FirmaBilgileridb;
            adres.VergiNo = vergiNo;
            adres.Type = firmayapısı;
            adres.FirmaAdı = TempData["FirmaAdi"].ToString();
            db.FirmaBilgileridb.Add(adres);
            db.SaveChanges();
            TempData["vergi"] = null;
            return adres.id;
        }
        public void addFirmaSahibi(int firmaid)
        {
            FirmaSahibidb sahip = TempData["sahip"] as FirmaSahibidb;
            sahip.Firmaid = firmaid;
            db.FirmaSahibidb.Add(sahip);
            db.SaveChanges();
           int id=sahip.id;
        }
        public void addMesulMüdür(int firmaid)
        {
            MesulMüdürüdb müdür = TempData["müdür"] as MesulMüdürüdb;
            müdür.Firmaid = firmaid;
            db.MesulMüdürüdb.Add(müdür);
            db.SaveChanges();
        }
        public void addFirmaDosyaları(int firmaid)
        {
            FirmaDosyalarıdb dosya = TempData["dosya"] as FirmaDosyalarıdb;
            dosya.Firmaid = firmaid;
            db.FirmaDosyalarıdb.Add(dosya);
            db.SaveChanges();
        }
        public void addDurum(int firmaid)
        {
            FirmaDurum durum = new FirmaDurum();
            int id = firmaid;
            durum.Firmaid = id;
            durum.Durum = 0;
            db.FirmaDurum.Add(durum);
            db.SaveChanges();
           

        }
        public void addPersonel(int firmaid)
        {
            List<PersonelBilgileridb> personel = TempData["Personeller"] as List<PersonelBilgileridb>;
           for (int i = 0; i < personel.Count; i++)
            {
                personel[i].Firmaid = firmaid;
                db.PersonelBilgileridb.Add(personel[i]);
            }
            db.SaveChanges();
            
        }
        public ActionResult Pdf()
        {
            int id = Int32.Parse(TempData["id"].ToString());
            ContextEntitiess context = new ContextEntitiess();
            return new ViewAsPdf(context.FirmaSahibidb.Where(p => p.Firmaid == id).ToList())
            {
                FileName = "MyPDF.pdf",
                CustomSwitches = "--print-media-type",
                PageSize = Rotativa.Options.Size.A4
            };

        }
    }

}