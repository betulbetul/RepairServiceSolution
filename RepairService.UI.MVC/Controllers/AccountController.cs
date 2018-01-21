using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
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
    public class AccountController : BaseController
    {
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userManager = MembershipTools.NewUserManager();
            var userStore = MembershipTools.NewUserStore();
            var checkUserTC = userStore.Context.Set<Musteri>().FirstOrDefault(x => x.TcNo==model.TcNo)?.TcNo;
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
                ActivationCode=activationCode
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
                    userManager.AddToRole(user.Id, IdentityRoles.Customer.ToString());
                    // Müşteri modele ekleme yapılacak...
                    var yeniMusteri = new Musteri()
                    {
                        UserID = user.Id,
                        TcNo = model.TcNo
                    };
                    MusteriRepo musteriRepo = new MusteriRepo();
                    int musteriSonuc = musteriRepo.Insert(yeniMusteri);
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailModel()
                    {
                        To = user.Email,
                        Subject = "SmartTV Repair Service - Aktivasyon",
                        Message = $"Merhaba {user.Name} {user.Surname} <br/>Hesabınızı aktifleştirmek için <b><a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a></b> tıklayınız."
                    });
                    return RedirectToAction("Login", "Account", new { userName = $"{user.UserName}" });

                }
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı kayıt işleminde hata oluştu!");
                return View(model);
            }
        }
        // GET: Account
        public ActionResult Login(string ReturnUrl, string userName)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.User.IsInRole("Passive"))
                {
                    ViewBag.sonuc = "Sistemi kullanabilmeniz için eposta adresinizi aktifleştirmeniz gerekmektedir.";
                }
                var url = ReturnUrl.Split('/');
                // admin/kullaniciduzenle/5
                // admin/kullanicilar
                if (url[1].ToLower().Contains("admin"))
                {
                    ViewBag.sonuc = "Bu alana yönetici hesabınızla girebilirsiniz. Lütfen yönetici bilgilerinizle giriş yapınız.";
                }
            }
            var model = new LoginViewModel() { ReturnUrl = ReturnUrl };
            ViewBag.username = userName;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userManager = MembershipTools.NewUserManager();
            var user = await userManager.FindAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adını ve şifreyi doğru girdiğinizden emin olunuz.");
                return View(model);
            }
            var authManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            }, userIdentity);
            if (string.IsNullOrEmpty(model.ReturnUrl))
                return RedirectToAction("Index", "Home");
            try
            {
                var url = model.ReturnUrl.Split('/');
                if (url.Length == 4)
                    return RedirectToAction(url[2], url[1], new { id = url[3] });
                else
                    return RedirectToAction(url[2], url[1]);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Profil()
        {
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            var model = new ProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Username = user.UserName
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profil(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);

                var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();

                ViewBag.sonuc = "Bilgileriniz güncelleşmiştir";

                var model2 = new ProfileViewModel()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName
                };
                return View(model2);
            }
            catch (Exception ex)
            {
                ViewBag.sonuc = ex.Message;
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(ProfileViewModel model)
        {
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor");
                return View("Profile");
            }
            try
            {
                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);

                var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
                user = userManager.Find(user.UserName, model.OldPassword);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış girildi");
                    return View("Profile");
                }
                await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(model.NewPassword));
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ViewBag.sonuc = "Güncelleştirme işleminde bir hata oluştu. " + ex.Message;
                throw;
            }
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoverPassword(string email)
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            try
            {
                var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.Email == email);
                if (sonuc == null)
                {
                    ViewBag.sonuc = "E mail adresiniz sisteme kayıtlı değil";
                    return View();
                }
                var randomPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
                await userStore.SetPasswordHashAsync(sonuc, userManager.PasswordHasher.HashPassword(randomPass));
                await SiteSettings.SendMail(new MailModel()
                {
                    To = sonuc.Email,
                    Subject = "Şifreniz Değişti",
                    Message = $"Merhaba {sonuc.Name} {sonuc.Surname} <br/>Yeni Şifreniz : <b>{randomPass}</b>"
                });
                ViewBag.sonuc = "Email adresinize yeni şifreniz gönderilmiştir";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.sonuc = "Sistemsel bir hata oluştu. Tekrar deneyiniz " + ex.Message;
                return View();
            }
        }
        
        #region Aktivasyon Activitation

        [HttpGet]
        public async Task<ActionResult> Activation(string code)
        {
            var userStore = MembershipTools.NewUserStore();
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.ActivationCode == code);
            if (sonuc == null)
            {
                ViewBag.sonuc = "Aktivasyon işlemi  başarısız";
                return View();
            }

            if (sonuc.EmailConfirmed)
            {
                ViewBag.sonuc = "E-Posta adresiniz zaten onaylı";
                return View();
            }
            sonuc.EmailConfirmed = true;
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();

            var userManager = MembershipTools.NewUserManager();
            var roleId = userManager.FindById(sonuc.Id)?.Roles.First().RoleId;
            var roleName = MembershipTools.NewRoleManager().FindById(roleId).Name;
            if (roleName == IdentityRoles.Passive.ToString())
            {
                userManager.RemoveFromRole(sonuc.Id, IdentityRoles.Passive.ToString());
                userManager.AddToRole(sonuc.Id, IdentityRoles.Customer.ToString());
            }
            ViewBag.sonuc = $"Merhaba {sonuc.Name} {sonuc.Surname} <br/> Aktivasyon işleminiz başarılı";
            await SiteSettings.SendMail(new MailModel()
            {
                To = sonuc.Email,
                Message = ViewBag.sonuc.ToString(),
                Subject = "Uyelik - Aktivasyon"
            });
            return View();
        }


        #endregion
    }

}