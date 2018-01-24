using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.BLL.Settings;
using RepairService.Entity.Enums;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
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
                    userManager.AddToRole(user.Id, IdentityRoles.Operator.ToString());
                    // Operator modele ekleme yapılacak...
                    var yeniOperator = new Operator()
                    {
                        UserID = user.Id,
                        TcNo = model.TcNo
                    };

                    int operaSonuc = await new OperatorRepo().InsertAsync(yeniOperator);
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Repair Service - Aktivasyon",
                        Message = $"Merhaba {user.Name} {user.Surname} | User Name: {user.UserName}<br/>Hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a></b> tıklayınız."
                    });

                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "Smart TV Repair Service - Yeni Şifreniz ",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Hesabınız admin tarafından oluşturuldu.<b> <br/>" +
                        $"Şifreniz: {model.Password} <br/>  <a href='{siteUrl}/Account/Login?userName={user.UserName}'>Giriş Yapmak için tıklayınız.</a></b>"
                    });
                    var operatorno = new OperatorRepo().GetById(yeniOperator.TcNo);
                    ViewBag.Sonuc = $"{operatorno?.ApplicationUser.Name + " " + operatorno?.ApplicationUser.Surname} yeni operatör olarak eklenmiştir.";
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
                    userManager.AddToRole(user.Id, IdentityRoles.TecnicalPerson.ToString());
                    // teknisyen modele ekleme yapılacak...
                    var yeniTeknisyen = new Teknisyen()
                    {
                        userID = user.Id,
                        TcNo = model.TcNo
                    };

                    int teknisyenSonuc = await new TeknisyenRepo().InsertAsync(yeniTeknisyen);
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "UyelikDB - Aktivasyon",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a></b> tıklayınız."
                    });

                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "ServiceDB - Yeni Şifreniz ",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Hesabınız admin tarafından oluşturuldu.<b> <br/>" +
                      $"Şifreniz: {model.Password} <br/>  <a href='{siteUrl}/Account/Login?userName={user.UserName}'>Giriş Yapmak için tıklayınız.</a></b>"
                    });

                    var tekno = new TeknisyenRepo().GetById(yeniTeknisyen.TcNo);
                    ViewBag.Sonuc = $"{tekno?.ApplicationUser.Name + " " + tekno?.ApplicationUser.Surname} yeni teknisyen olarak eklenmiştir.";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı kayıt işleminde hata oluştu!");
                return View(model);
            }
            return View(new RegisterViewModel());

        }

    }


}