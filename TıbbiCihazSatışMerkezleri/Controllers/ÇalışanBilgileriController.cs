using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TıbbiCihazSatışMerkezleri.Control;
using TıbbiCihazSatışMerkezleri.Models;

namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class ÇalışanBilgileriController : Controller
    {
        // GET: ÇalışanBilgileri
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form ,HttpPostedFileBase FirmaSahibiPdf, HttpPostedFileBase MüdürSertifika)
        {
            ContextEntitiess context = new ContextEntitiess();
            string meslmüdürüTcno = form["MüdürTcNo"];
            if (context.MesulMüdürüdb.Where(p => p.TcNo == meslmüdürüTcno).Count() == 0)
            {
                FirmaSahibidb sahip = new FirmaSahibidb();
                sahip.TcNo = form["firmaTcNo"];
                sahip.AdıSoyadı = form["firmaAdıSoyadı"];
                TempData["sahip"] = sahip;
                if (FirmaSahibiPdf != null)
                {
                    sahip.HizmetSözleşmesi = new byte[FirmaSahibiPdf.ContentLength];
                    FirmaSahibiPdf.InputStream.Read(sahip.HizmetSözleşmesi, 0, FirmaSahibiPdf.ContentLength);
                }
                MesulMüdürüdb müdür = new MesulMüdürüdb();
                müdür.TcNo = meslmüdürüTcno;
                müdür.AdıSoyadı = form["MüdürAdSoyad"];
                müdür.SertifikaNo = form["MüdürSertifikaNo"];
                müdür.SertfikaTarihi = form["MüdürSertifikaTarihi"];
                TempData["müdür"] = müdür;
                if (MüdürSertifika != null)
                {
                    müdür.SertifikaDosyası = new byte[MüdürSertifika.ContentLength];
                    MüdürSertifika.InputStream.Read(müdür.SertifikaDosyası, 0, MüdürSertifika.ContentLength);
                }
                int personelsayısı = Int32.Parse(form["item"]);
                for (int i = 1; i < personelsayısı + 1; i++)
                {

                    HttpPostedFileBase personeldosyası = Request.Files["PersonelFile" + i + ""];
                    Personeller(Request.Form.GetValues("personel" + i + ""), personeldosyası);
                }
                return RedirectToAction("Index", "FirmaDosyaları");
               
            }
            else
            {
                TempData["MesulMüdür"] = "Mesul Müdürünüz Başka Bir Firmada Çalışmaktadır";
                return View();
            }
        }
        List<PersonelBilgileridb> personeller = new List<PersonelBilgileridb>();
        public void Personeller(string[] personelbilgileri, HttpPostedFileBase personeldosyası)
        {
            if (personelbilgileri != null && personeldosyası != null) { 
                PersonelBilgileridb personel = new PersonelBilgileridb();
                personel.TcNo = personelbilgileri[0];
                personel.AdıSoyadı = personelbilgileri[1];
                personel.SertifikaNo = personelbilgileri[2];
                personel.SertifikaTarihi = personelbilgileri[3];
                personel.SertifikaTürü = personelbilgileri[4];
                personel.SertifikaDosyası = new byte[personeldosyası.ContentLength];
                personeldosyası.InputStream.Read(personel.SertifikaDosyası, 0, personeldosyası.ContentLength);
                personeller.Add(personel);
            }

            TempData["Personeller"] = personeller;
        }
    }
}