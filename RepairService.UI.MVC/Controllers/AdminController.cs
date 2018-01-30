using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.BLL.Settings;
using RepairService.Entity.Enums;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
using RepairService.Entity.Models.Kisi;
using RepairService.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{
    // Sadece Admin erişebilir. // 
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        const int pageSize = 24;

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Musteriler(int? page = 1)
        {
            MusteriRepo repoMusteri = new MusteriRepo();
            var musteriler = repoMusteri.GetAll()
                .Skip((page.Value < 1 ? 1 : page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var total = repoMusteri.GetAll().Count();
            ViewBag.ToplamSayfa = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.Suan = page;
            return View(musteriler);
        }
        public ActionResult MusteriDetay(string tcno)
        {
            if (tcno == null)
                return RedirectToAction("Musteriler");
            var musteri = new MusteriRepo().GetById(tcno);
            return View(musteri);
        }

        [HttpGet]
        public ActionResult OperatorEkle()
        {
            var model = new RegisterViewModel();
            model.Password = RandomYeniSifreOlustur();
            model.ConfirmPassword = model.Password;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OperatorEkle(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var userManager = MembershipTools.NewUserManager();
            var userStore = MembershipTools.NewUserStore();
            var checkUserTC = userStore.Context.Set<Operator>().FirstOrDefault(x => x.TcNo == model.TcNo)?.TcNo;
            if (checkUserTC != null)
            {
                ModelState.AddModelError(string.Empty, "Bu TC Numarası daha önceden kayıt edilmiştir!");
                return View(model);
            }
            var checkUserName = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.UserName == model.Username)?.UserName;
            if (checkUserName != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı başka bir üye tarafından alınmıştır!");
                return View(model);
            }
            var checkUser = userManager.FindByName(model.Username);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı daha önceden kayıt edilmiş");
                return View(model);
            }
            var activationCode = Guid.NewGuid().ToString().Replace("-", "");

            var user = new ApplicationUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ActivationCode = activationCode
            };

            var sonuc = userManager.Create(user, model.Password);
            if (sonuc.Succeeded)
            {
                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, IdentityRoles.Admin.ToString());
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    userManager.AddToRole(user.Id, IdentityRoles.Passive.ToString());
                    // Öncelikle pasif olarak eklenecek. Kendi emailini aktifleştirmeye tıklayınca Operator olacak.
                    var yeniPasifKisi = new Pasif()
                    {
                        UserID = user.Id,
                        TcNo = model.TcNo
                    };

                    int pasifSonuc = await new PasifRepo().InsertAsync(yeniPasifKisi);
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Repair Service - Operator Aktivasyon İşlemi",
                        Message = $"Merhaba {user.Name} {user.Surname} | User Name: {user.UserName}<br/>Operatör hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/OperatorActivation?code={activationCode}'>Operatör Aktivasyon Kodu</a></b> tıklayınız."
                    });

                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Teknis Servis - Yeni Şifreniz ",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Pasif olan operatör hesabınız admin tarafından oluşturuldu.<b> <br/>" +
                        $"Şifreniz: {model.Password} <br/>  <a href='{siteUrl}/Account/Login?userName={user.UserName}'>Giriş Yapmak için tıklayınız.</a></b>"
                    });
                    var operatorno = new OperatorRepo().GetById(yeniPasifKisi.TcNo);
                    ViewBag.Sonuc = $"{operatorno?.ApplicationUser.Name + " " + operatorno?.ApplicationUser.Surname} yeni operatör (pasif hesap) olarak eklenmiştir.";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı kayıt işleminde hata oluştu!");
                return View(model);
            }
            return View(new RegisterViewModel());
        }

        public ActionResult TeknisyenEkle()
        {
            var model = new RegisterViewModel();
            model.Password = RandomYeniSifreOlustur();
            model.ConfirmPassword = model.Password;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeknisyenEkle(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userManager = MembershipTools.NewUserManager();
            var userStore = MembershipTools.NewUserStore();
            var checkUserTC = userStore.Context.Set<Teknisyen>().FirstOrDefault(x => x.TcNo == model.TcNo)?.TcNo;
            if (checkUserTC != null)
            {
                ModelState.AddModelError(string.Empty, "Bu TC Numarası daha önceden kayıt edilmiştir!");
                return View(model);
            }
            var checkUserName = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.UserName == model.Username)?.UserName;
            if (checkUserName != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı başka bir üye tarafından alınmıştır!");
                return View(model);
            }

            var checkUser = userManager.FindByName(model.Username);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı daha önceden kayıt edilmiş");
                return View(model);
            }
            var activationCode = Guid.NewGuid().ToString().Replace("-", "");

            var user = new ApplicationUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ActivationCode = activationCode
            };

            var sonuc = userManager.Create(user, model.Password);
            if (sonuc.Succeeded)
            {
                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, IdentityRoles.Admin.ToString());
                    return RedirectToAction("Index", "Admin");
                }

                else
                {
                    userManager.AddToRole(user.Id, IdentityRoles.Passive.ToString());
                    // Öncelikle pasif olarak eklenecek. Kendi emailini aktifleştirmeye tıklayınca Teknisyen olacak.
                    var yeniPasifKisi = new Pasif()
                    {
                        UserID = user.Id,
                        TcNo = model.TcNo
                    };

                    int pasifSonuc = await new PasifRepo().InsertAsync(yeniPasifKisi);
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Teknik Servis - Teknik Personel Aktivasyon İşlemi",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Pasif olan teknisyen hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/TeknisyenActivation?code={activationCode}'>Teknisyen Aktivasyon Kodu</a></b> tıklayınız."
                    });

                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Teknik Servis - Yeni Şifreniz ",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Pasif olan teknisyen hesabınız admin tarafından oluşturuldu.<b> <br/>" +
                      $"Şifreniz: {model.Password} <br/>  <a href='{siteUrl}/Account/Login?userName={user.UserName}'>Giriş Yapmak için tıklayınız.</a></b>"
                    });

                    var tekno = new TeknisyenRepo().GetById(yeniPasifKisi.TcNo);
                    ViewBag.Sonuc = $"{tekno?.ApplicationUser.Name + " " + tekno?.ApplicationUser.Surname} yeni teknisyen (pasif hesap) olarak eklenmiştir.";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı kayıt işleminde hata oluştu!");
                return View(model);
            }
            return View(new RegisterViewModel());

        }

        public ActionResult TumOperatorler(int? page = 1)
        {
            OperatorRepo repoOperator = new OperatorRepo();
            var operatorler = repoOperator.GetAll()
                .Skip((page.Value < 1 ? 1 : page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var total = repoOperator.GetAll().Count();
            ViewBag.ToplamSayfa = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.Suan = page;
            return View(operatorler);
        }
        public ActionResult TumTeknisyenler(int? page = 1)
        {
            TeknisyenRepo repoTeknisyen = new TeknisyenRepo();
            var teknisyenler = repoTeknisyen.GetAll()
                .Skip((page.Value < 1 ? 1 : page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var total = repoTeknisyen.GetAll().Count();
            ViewBag.ToplamSayfa = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.Suan = page;
            return View(teknisyenler);

        }
        public ActionResult Istatistikler()
        {
            ServisKaydiRepo repoServisKaydi = new ServisKaydiRepo();
            //Çözümlenen kayıt oranı
            var successServisler = repoServisKaydi.GetAll().Where(x => x.Durumu == ArizaDurum.Cozuldu);
            var servisToplamSayi = repoServisKaydi.GetAll().Count;
            var successToplamSayi = successServisler.Count();
            double successOran = (successToplamSayi * 100) / successToplamSayi;
            ViewBag.SuccessOran = successOran;
            return View();
        }

        public ActionResult Anketler()
        {
            ViewBag.Anketler = new AnketRepo().GetAll().ToList();
            return View();
        }
    }


}