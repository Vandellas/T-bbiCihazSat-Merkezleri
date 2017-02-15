using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Control;
using TıbbiCihazSatışMerkezleri.Models;
namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class FirmaDosyalarıController : BaseControl
    {
        // GET: FirmaDosyaları
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form, HttpPostedFileBase ttsgo, HttpPostedFileBase vlo, HttpPostedFileBase iso,HttpPostedFileBase iyavçr)
        {
            FirmaDosyalarıdb dosya = new FirmaDosyalarıdb();
            if (form["checkSüre"] == "on")
                dosya.işyeriaçma = "Evet";
            else
                dosya.işyeriaçma = "Hayır";
            if (ttsgo != null)
            {
                dosya.TürkiyeTicaretSicilGazetesiOrnegi = new byte[ttsgo.ContentLength];
                ttsgo.InputStream.Read(dosya.TürkiyeTicaretSicilGazetesiOrnegi, 0, ttsgo.ContentLength);
            }
           if (iyavçr != null)
            {
                dosya.İşYeriAçmaveÇalıştırmaRuhsatı = new byte[iyavçr.ContentLength];
                iyavçr.InputStream.Read(dosya.İşYeriAçmaveÇalıştırmaRuhsatı, 0, iyavçr.ContentLength);
            }
            if (vlo != null)
            {
                dosya.VergiLevhasıOrnegi = new byte[vlo.ContentLength];
                vlo.InputStream.Read(dosya.VergiLevhasıOrnegi, 0, vlo.ContentLength);
            }
            if (iso != null)
            {
                dosya.İmzaSirküleriOrnegi = new byte[iso.ContentLength];
                iso.InputStream.Read(dosya.İmzaSirküleriOrnegi, 0, iso.ContentLength);
            }
            dosya.İşyeriRuhsatTarihi = form["irt"];
            dosya.İşyeriRuhsatNo = form["irn"];
            dosya.ÇalışmaAlanı = form["ÇalışmaAlanı"];
            dosya.DenemeOdası = form["DenemeOdası"];
            TempData["dosya"] = dosya;
            return RedirectToAction("Index","Taahhütname");
        }
    }
}