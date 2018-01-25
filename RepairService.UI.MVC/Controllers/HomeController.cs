using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.BLL.Settings;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{
    public class HomeController : BaseController
    {
        const int customerPageSize = 6;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public ActionResult ArizaKaydiOlustur()
        {
            CihazTuruRepo cihazTuruRepo = new CihazTuruRepo();
            var cihazTurleri = new List<SelectListItem>();
            cihazTuruRepo.GetAll().ToList().ForEach(x => cihazTurleri.Add(new SelectListItem()
            {
                Text = x.Tur,
                Value = x.Id.ToString()
            }));
            ViewBag.CihazTurleri = cihazTurleri;

            ArizaTuruRepo arizaTuruRepo = new ArizaTuruRepo();
            var arizaTurleri = new List<SelectListItem>();
            arizaTuruRepo.GetAll().ToList().ForEach(x => arizaTurleri.Add(new SelectListItem()
            {
                Text = x.TurAdi.ToString(),
                Value = x.Id.ToString()

            }));
            ViewBag.ArizaTurleri = arizaTurleri;

            return View(new ServisKaydiViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ArizaKaydiOlustur(ServisKaydiViewModel model)
        {
            //Cihaz modeli veritabanından kontrol et
            var cihazModel = new CihazModelRepo().GetAll().Where(x => x.MarkaAdi == model.CihazMarka && x.ModelAdi == model.CihazModel && x.CihazTuruId.ToString() == model.CihazTuru.ToString()).FirstOrDefault();
            var cihazTuru = new CihazTuruRepo().GetAll().Where(x => x.Id.ToString() == model.CihazTuru.ToString()).FirstOrDefault();
            var arizaTuru = new ArizaTuruRepo().GetAll().Where(x => x.Id.ToString() == model.ArizaTurAdi.ToString()).FirstOrDefault();
            var musteri = new MusteriRepo().GetAll().Where(x => x.UserID == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            ServisKaydi yeniServisKaydi = new ServisKaydi();
            string servisNo = RandomServisKaydıNumarasiOlustur();
            //servisNo kontrol edilsin.
            if (new ServisKaydiRepo().GetAll().Where(x => x.ServisNumarasi == servisNo).Any())
                servisNo = RandomServisKaydıNumarasiOlustur();
            else
            {
                if (cihazModel == null)
                {
                    try
                    {
                        CihazModel yeniCihazModel = new CihazModel()
                        {
                            MarkaAdi = model.CihazMarka,
                            ModelAdi = model.CihazModel,
                            CihazTuruId = cihazTuru.Id,
                            EklenmeTarihi = DateTime.Now
                        };
                        new CihazModelRepo().Insert(yeniCihazModel);
                        yeniServisKaydi = new ServisKaydi()
                        {
                            MusteriTCNo = musteri.TcNo,
                            ArizaTuruId = arizaTuru.Id,
                            CihazModelId = yeniCihazModel.Id,
                            MusteriArizaTanimi = model.musteriArizaTanimi,
                            MusteriUcretiOnayladiMi = false,
                            Durumu = Entity.Enums.ArizaDurum.onayBekliyor,
                            Fiyat = 0m,
                            EklenmeTarihi = DateTime.Now,
                            KonumLat = model.KonumLat,
                            KonumLng = model.KonumLng,
                            AcikAdres = model.AcikAdres,
                            Telefon = model.Telefon
                        };
                        new ServisKaydiRepo().Insert(yeniServisKaydi);
                        yeniServisKaydi.ServisNumarasi = servisNo;
                        await new ServisKaydiRepo().UpdateAsync();
                        ViewBag.ServisNo = yeniServisKaydi.ServisNumarasi;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
                else
                {
                    //Cihazmodel tablosunda bu cihazdan zaten varsa...
                    try
                    {
                        yeniServisKaydi = new ServisKaydi()
                        {
                            MusteriTCNo = musteri.TcNo,
                            ArizaTuruId = arizaTuru.Id,
                            CihazModelId = cihazModel.Id,
                            Durumu = Entity.Enums.ArizaDurum.onayBekliyor,
                            EklenmeTarihi = DateTime.Now,
                            MusteriArizaTanimi = model.musteriArizaTanimi,
                            MusteriUcretiOnayladiMi = false,
                            Fiyat = 0m,
                            KonumLat = model.KonumLat,
                            KonumLng = model.KonumLng,
                            AcikAdres = model.AcikAdres,
                            Telefon = model.Telefon
                        };
                        new ServisKaydiRepo().Insert(yeniServisKaydi);
                        yeniServisKaydi.ServisNumarasi = servisNo;
                        await new ServisKaydiRepo().UpdateAsync();
                        ViewBag.ServisNo = yeniServisKaydi.ServisNumarasi;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                //Dosyaları da kayıt edecek...
                if (model.Dosyalar.Any())
                {

                    foreach (var dosya in model.Dosyalar)
                    {
                        if (dosya != null && dosya.ContentType.Contains("image") && dosya.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(dosya.FileName);
                            string extName = Path.GetExtension(dosya.FileName);
                            fileName = SiteSettings.UrlFormatConverter(fileName);
                            fileName += Guid.NewGuid().ToString().Replace("-", "");
                            var directoryPath = Server.MapPath($"~/ServicePictures/{user.UserName}/{yeniServisKaydi.ServisNumarasi}");
                            var filePath = Server.MapPath($"~/ServicePictures/{user.UserName}/{servisNo}/") + fileName + extName;
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            dosya.SaveAs(filePath);
                            ResimBoyutlandir(600, 600, filePath);
                            await new DosyaRepo().InsertAsync(new Dosya()
                            {
                                DosyaYolu = @"/ServicePictures/" + user.UserName + "/" + yeniServisKaydi.ServisNumarasi + "/" + fileName + extName,
                                Uzanti = extName.Substring(1),
                                arizaId = yeniServisKaydi.Id,
                                EklenmeTarihi = DateTime.Now
                            });
                        }
                    }
                }
                //Mail gönder
                await SiteSettings.SendMail(new MailModel()
                {
                    To = user.Email,
                    Subject = "SmartTV Repair Service - Servis Kaydı",
                    Message = $"Merhaba {user.Name} {user.Surname} <br/>Servis Kaydı Referans Numaranız: {servisNo}<b>"
                });
            }


            return View();
        }

        public ActionResult Iletisim()
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId()) ?? null;
            if (user != null)
                ViewBag.IsimSoyisim = user.Name + " " + user.Surname;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Iletisim(IletisimMailModel model, string isimSoyisim)
        {
            await SiteSettings.SendMail(new IletisimMailModel()
            {
                IsimSoyisim = model.IsimSoyisim,
                EmailAdresi = model.EmailAdresi,
                Konu = model.Konu,
                Mesaj = model.Mesaj,
                Cc = "betulaksan34@gmail.com"
            });
            ViewBag.Durum = true;
            return View(new IletisimMailModel());
        }

        [Authorize(Roles = "Customer")]

        public ActionResult MusteriServisKayitlari(int? page = 1)
        {
            //Musteri
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            //Musteriye ait servis kayıtları
            var serviskayitlari = new ServisKaydiRepo().GetAll().Where(x => x.Musteri.UserID == user.Id)
                 .Skip((page.Value < 1 ? 1 : page.Value - 1) * customerPageSize)
                .Take(customerPageSize)
                .ToList();
            List<ServisKaydiIslem> servisKaydiTeknisyenIslemleriList = new List<ServisKaydiIslem>();
            List<Dosya> servisKaydiDosyalari = new List<Dosya>();
            foreach (var item in serviskayitlari)
            {
                //tum serviskaydi teknisyen işlemleri listesi
                servisKaydiTeknisyenIslemleriList = new ServisKaydiIslemRepo().GetAll().Where(x => x.ServisId == item.Id).ToList();
                servisKaydiDosyalari = new DosyaRepo().GetAll().Where(x => x.arizaId == item.Id).ToList();
            }
            ViewBag.TeknisyenIslemleri = servisKaydiTeknisyenIslemleriList;
            ViewBag.ServisKaydiDosyalari = servisKaydiDosyalari;
            var total = new ServisKaydiRepo().GetAll().Count();
            ViewBag.ToplamSayfa = (int)Math.Ceiling(total / (double)customerPageSize);
            ViewBag.Suan = page;
            ViewBag.Total = new ServisKaydiRepo().GetAll().Where(x => x.Musteri.UserID == user.Id).ToList().Count();
            return View(serviskayitlari);
        }
        [HttpGet]
        public ActionResult MusteriServisDetay(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction("MusteriServisKayitlari");
            }
            else
            {
                var serviskayitlari = new ServisKaydiRepo().GetAll().Where(x => x.Id == id).ToList();
                return View(serviskayitlari);
            }
        }
    }
}