using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TıbbiCihazSatışMerkezleri.Models;

namespace TıbbiCihazSatışMerkezleri.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.check = "checked";
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var response = Request["g-recaptcha-response"];
            var secretkey = "6LdKehIUAAAAAO_YaPgYHLVvnvr-rVIHnZFhrzl9";
            var Client = new WebClient();
            var result = Client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretkey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
                string vergiNo = form["vergiNo"].ToString();
                ViewBag.vergiNo = vergiNo;
                var db = new ContextEntitiess();
                bool kontrol = true;
                if (form["type"] == "0")
                {
                    ViewBag.check = "checked";
                    kontrol = FirmaKontrol(vergiNo, "0");
                }
                else
                {
                    ViewBag.check = "";
                    ViewBag.checksube = "checked";
                    kontrol = FirmaKontrol(vergiNo, "1");
                    ViewBag.Subekodu = "1";
                }
             if (status == true)
             {
                Vergi model = GetVergi(vergiNo);
                if (model != null)
                { 

                    if (kontrol == false)
                    {
                        ViewBag.visibility = 0;
                        Session["User"] = "1";
                    }
                    else
                    {
                        string firmayapısı = form["type"].ToString();
                        if(firmayapısı=="0")
                        TempData["SorguDurumu"] = "Bu firma Sistemimizde mevcut.";
                        else
                            TempData["SorguDurumu"] = "Merkeziniz Bulunmamaktadır.Lütfen Önce Merkezini Kaydediniz.";
                    }
                    ViewBag.getDb = 0;
                    ViewBag.FirmaAdi = model.FirmaAdi;
                    ViewBag.adres = model.FirmaGenelMüdürlükAdresi;
                    ViewBag.vergiDairesi = model.VergiDairesi;
                    TempData["FirmaAdi"] = model.FirmaAdi;
                }
                else 
                {
                    TempData["SorguDurumu"] = "Lütfen Vergi Numaranızı kontrol ediniz";
                }
            }
            else
               TempData["CaptchaDurumu"] = "Lütfen robot olmadığını onayla.";
            return View();
        }
        public Vergi GetVergi(String vergiNo)
        {
            var db = new ContextEntitiess();//Veri tabanımızı çağırdık
            Vergi vergi = db.Vergi.Where(q => q.VergiNumarası == vergiNo).FirstOrDefault();
            return vergi;
        }
        public bool FirmaKontrol(String vergiNo, String firmayapısı)
        {
            TempData["vergi"] = vergiNo;
            TempData["firmayapısı"] = firmayapısı;
            var db = new ContextEntitiess();//Veri tabanımızı çağırdık
            int count = db.FirmaBilgileridb.Where(q => q.VergiNo == vergiNo && q.Type == "0"&&q.FirmaDurum.FirstOrDefault().Durum!=3).Count();
            if (firmayapısı == "0")
            {
                if (count == 0)
                    return false;
                else
                    return true;
            }
            else
            {
                if (count > 0)
                    return false;
                else
                   return true;
            }
               
        }
        public ActionResult OnayDurumu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OnayDurumu(FormCollection form)
        {
            ContextEntitiess context = new ContextEntitiess();
            
            
            if (form["sbmtbtn"] == "0")
            {
                string vergiNo = form["VergiNumarası"];
                int onayKodu = Int32.Parse(form["OnayKodu"]);
                
                FirmaDurum durum = context.FirmaDurum.Where(p => p.FirmaBilgileridb.VergiNo == vergiNo && p.Kontrolid == onayKodu).FirstOrDefault();
               
                if (durum == null)
                    TempData["durumiptal"] = "Böyle bir kayıt bulunamadı lütfen Vergi Numaranız ve Onay Kodunuzu kontrol ediniz";
                else
                {
                    TempData["durumid"] = durum.Kontrolid;
                    var Bilgiler = context.FirmaBilgileridb.Where(p => p.id == durum.Firmaid).ToList();
                    if (durum.Durum == 0)
                        TempData["OnayDurumu"] = "Kaydınız beklemededir Lütfen sonra tekrar deneyin";
                    else if (durum.Durum == 1)
                        TempData["OnayDurumu"] = "Kaydınız Onaylanmıştır";
                    else if (durum.Durum == 2)
                    {
                        TempData["Güncelle"]= Bilgiler[0].id;
                        TempData["OnayDurumu"] = "Kaydınız Reddedilmiştir\n Nedeni:" + durum.RedSebebi;
                    }
                    else
                        TempData["OnayDurumu"] = "Kaydınız İptal Edilmiştir";
                    return View(Bilgiler);
                }
            } 
            else
            {
            
                 FirmaDurum durumdegis = new FirmaDurum();
                  durumdegis = context.FirmaDurum.Find(TempData["durumid"]);
                  durumdegis.Durum = 3;
                  durumdegis.Tarih = DateTime.Now;
                  context.Entry(durumdegis).State = EntityState.Modified;
                  context.SaveChanges();
                  TempData["durumiptal"] = "İşleminiz İptal Edilmiştir";

            }
            return View();
        }
        public ActionResult Güncelle()
        {
            int Firmaid = -1;
            try
            {
                Firmaid = Int32.Parse(TempData["Güncelle"].ToString());
                Session["Firmaid"] = Firmaid;
               
            }
            catch
            {

            }
            List<object> Model = new List<object>();
            ContextEntitiess context = new ContextEntitiess();
            Model.Add(context.FirmaSahibidb.Where(p => p.Firmaid == Firmaid).ToList());
            Model.Add(context.iller.ToList());
            ViewBag.illist = new SelectList(context.iller, "sehirid", "sehir");
            return View(Model);
        }
        [HttpPost]
        public ActionResult Güncelle(FormCollection form)
        {
            int Firmaid = Int32.Parse(Session["Firmaid"].ToString());
            ContextEntitiess context = new ContextEntitiess();
            FirmaSahibidb sahip = new FirmaSahibidb();
            sahip = context.FirmaSahibidb.Find(Firmaid);
            sahip.AdıSoyadı = form["firmaAdıSoyadı"];
            sahip.TcNo = form["firmaTcNo"];
            if (Request.Files["yhs"].FileName != "")
            {
                sahip.HizmetSözleşmesi = new byte[Request.Files["yhs"].ContentLength];
                Request.Files["yhs"].InputStream.Read(sahip.HizmetSözleşmesi, 0, Request.Files["yhs"].ContentLength);
            }
            sahip.FirmaBilgileridb.Cadde = form["Cadde"];
            sahip.FirmaBilgileridb.DışKapıNo = form["DışKapıNo"];
            sahip.FirmaBilgileridb.FaxNo = form["FaxNo"];
            sahip.FirmaBilgileridb.FirmaEposta = form["Eposta"];
            int ilid = Int32.Parse(form["il"]);
            sahip.FirmaBilgileridb.il = context.iller.Where(q => q.sehirid == ilid).Select(q => q.sehir).FirstOrDefault();
            int ilceid = Int32.Parse(form["ilce"]);
            sahip.FirmaBilgileridb.ilce = context.ilceler.Where(q => q.id == ilceid).Select(q => q.ilce).FirstOrDefault();
            sahip.FirmaBilgileridb.Mahalle = form["Mahalle"];
            sahip.FirmaBilgileridb.PostaKodu = form["PostaNo"];
            sahip.FirmaBilgileridb.Sokak = form["Sokak"];
            sahip.FirmaBilgileridb.TelNo = form["TelNo"];
            sahip.FirmaBilgileridb.TelNo2 = form["TelNo2"];
            sahip.FirmaBilgileridb.VergiDairesi = form["VergiDaire"];
            sahip.FirmaBilgileridb.WebSite = form["WebSite"];
            sahip.FirmaBilgileridb.İçKapıNo = form["İçKapıNo"];
            sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().DenemeOdası = form["DenemeOdası"];
            sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().ÇalışmaAlanı = form["ÇalışmaAlanı"];
            sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İşyeriRuhsatNo = form["irn"];
            sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İşyeriRuhsatTarihi = form["irt"];
            if (form["check"] == "on")
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().işyeriaçma = "Evet";
            else
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().işyeriaçma = "Hayır";
            if (Request.Files["iyçr"].FileName != "")
            {
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İşYeriAçmaveÇalıştırmaRuhsatı = new byte[Request.Files["iyçr"].ContentLength];
                Request.Files["iyçr"].InputStream.Read(sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İşYeriAçmaveÇalıştırmaRuhsatı, 0, Request.Files["iyçr"].ContentLength);
            }
            if (Request.Files["ttsgo"].FileName != "")
            {
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().TürkiyeTicaretSicilGazetesiOrnegi = new byte[Request.Files["ttsgo"].ContentLength];
                Request.Files["ttsgo"].InputStream.Read(sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().TürkiyeTicaretSicilGazetesiOrnegi, 0, Request.Files["ttsgo"].ContentLength);
            }
            if (Request.Files["vlo"].FileName != "")
            {
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().VergiLevhasıOrnegi = new byte[Request.Files["vlo"].ContentLength];
                Request.Files["vlo"].InputStream.Read(sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().VergiLevhasıOrnegi, 0, Request.Files["vlo"].ContentLength);
            }
            if (Request.Files["iso"].FileName != "")
            {
                sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İmzaSirküleriOrnegi = new byte[Request.Files["iso"].ContentLength];
                Request.Files["iso"].InputStream.Read(sahip.FirmaBilgileridb.FirmaDosyalarıdb.FirstOrDefault().İmzaSirküleriOrnegi, 0, Request.Files["iso"].ContentLength);
            }
            sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().TcNo = form["MüdürTcNo"];
            sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().AdıSoyadı = form["MüdürAdSoyad"];
            sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().SertfikaTarihi = form["MüdürSertifikaTarihi"];
            sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().SertifikaNo = form["MüdürSertifikaNo"];
            if (Request.Files["msd"].FileName != "")
            {
                sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().SertifikaDosyası = new byte[Request.Files["msd"].ContentLength];
                Request.Files["msd"].InputStream.Read(sahip.FirmaBilgileridb.MesulMüdürüdb.FirstOrDefault().SertifikaDosyası, 0, Request.Files["msd"].ContentLength);
            }
            for (int i = 0; i < sahip.FirmaBilgileridb.PersonelBilgileridb.Count(); i++)
            {
                 string[] personelbilgileri = Request.Form.GetValues("personel" + (i + 1));
                sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).TcNo = personelbilgileri[0];
                sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).AdıSoyadı = personelbilgileri[1];
                sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).SertifikaNo = personelbilgileri[2];
                sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).SertifikaTarihi = personelbilgileri[3];
                sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).SertifikaTürü = personelbilgileri[4];
                if (Request.Files["PersonelFile" + (i + 1)].FileName != "")
                {
                    HttpPostedFileBase personeldosyası = Request.Files["PersonelFile" + (i + 1)];
                    sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).SertifikaDosyası = new byte[personeldosyası.ContentLength];
                    personeldosyası.InputStream.Read(sahip.FirmaBilgileridb.PersonelBilgileridb.ElementAt(i).SertifikaDosyası, 0, personeldosyası.ContentLength);
                }

            }
            context.Entry(sahip).State = EntityState.Modified;
            context.SaveChanges();
            TempData["Onaylama"] = "İşleminiz Gerçekleşmiştir";
            List<object> Model = new List<object>();
            Model.Add(context.FirmaSahibidb.Where(p => p.Firmaid == Firmaid).ToList());
            Model.Add(context.iller.ToList());
            ViewBag.illist = new SelectList(context.iller, "sehirid", "sehir");
            return View(Model);
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

    }
}