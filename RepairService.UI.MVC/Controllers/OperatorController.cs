using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.BLL.Settings;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{


    // Sadece Operator erişebilir. // 
    [Authorize(Roles = "Operator")]

    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Musteriler()
        {
            return View();
        }
        public ActionResult YeniServisKayitlari()
        {
            //onay bekliyor olan servis kayıtları 
            var yeniServisKayitlari = new ServisKaydiRepo().GetAll().Where(x => x.Durumu == Entity.Enums.ArizaDurum.onayBekliyor).ToList();
            return View(yeniServisKayitlari);
        }
        public async Task<ActionResult> YeniServisKayitlariKabulEt(int? id)
        {
            if (id == null)
                return RedirectToAction("YeniServisKayitlari");
            var servisKaydi = new ServisKaydiRepo().GetAll().FirstOrDefault(x => x.Id == id);
            if (servisKaydi == null)
                return RedirectToAction("YeniServisKayitlari");
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId()) ?? null;
            var operaSonuc = new OperatorRepo().GetAll().FirstOrDefault(x => x.UserID == user.Id);

            servisKaydi.OperatorTCNo = operaSonuc.TcNo;
            servisKaydi.Durumu = Entity.Enums.ArizaDurum.operatoreAktarildi;
            new ServisKaydiRepo().Update();
            TempData["Sonuc"] = true;
            TempData["ServisNo"] = servisKaydi.ServisNumarasi;
            TempData["Operator"] = operaSonuc.ApplicationUser.UserName;
            await SiteSettings.SendMail(new MailModel()
            {
                To = user.Email,
                Subject = $"SmartTV Repair Service - Servis Kaydı Onayı  ",
                Message = $"Merhaba operator {user.UserName} <br/>Yeni servis kaydını kabul ettiniz.<b> <br/> Servis Kaydı Numarası: {servisKaydi.ServisNumarasi} <br/> <a href='{Url.Action("Login", "Account")}'>Sisteme Giriş Yapmak için Tıkla...</a>"
            });
            return View();
        }

        public ActionResult OperatorunServisleri()
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId()) ?? null;
            var operaSonuc = new OperatorRepo().GetAll().FirstOrDefault(x => x.UserID == user.Id);
            // Operatore ait tüm servisler
            var operatorServisler = new ServisKaydiRepo().GetAll().Where(x => x.OperatorTCNo == operaSonuc.TcNo).ToList();

            return View(operatorServisler); // Operatore ait tüm servisler
        }
        public ActionResult TeknisyeneServisKaydiGonder()
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId()) ?? null;
            var operaSonuc = new OperatorRepo().GetAll().FirstOrDefault(x => x.UserID == user.Id);
            //Teknisyeni olmayan operatöre ait servisler
            var operatorServisler = new ServisKaydiRepo().GetAll().Where(x => x.OperatorTCNo == operaSonuc.TcNo && x.TeknisyenTCNo == null).ToList();

            //Boştaki teknisyenleri gönder
            var tumServisler = new ServisKaydiRepo().GetAll().ToList();
            var tumTeknisyenler = new TeknisyenRepo().GetAll().ToList();
            List<Teknisyen> bostakiTeknisyenler = tumTeknisyenler;
            for (int i = 0; i < tumTeknisyenler.Count; i++)
            {
                for (int k = 0; k < tumServisler.Count; k++)
                {
                    if (tumServisler[k].TeknisyenTCNo == tumTeknisyenler[i].TcNo)
                    {
                        if (tumServisler[i].Durumu != Entity.Enums.ArizaDurum.Cozuldu)
                            bostakiTeknisyenler.Remove(tumTeknisyenler[i]);

                    }
                }
            }
            var bosTek = new List<SelectListItem>();
            bostakiTeknisyenler.ForEach(x => bosTek.Add(new SelectListItem()
            {
                Text = x.ApplicationUser.UserName,
                Value = x.userID.ToString()
            }));
            ViewBag.BosTeknisyenler = bosTek;


            return View(operatorServisler);  //Teknisyeni olmayan operatöre ait servisler

        }

        [HttpPost]
        public async Task<ActionResult> TeknisyeneServisKaydiGonder(ServisKaydi gelenModel)
        {
            ServisKaydiRepo repoServisKaydi = new ServisKaydiRepo();
            var servis = repoServisKaydi.GetById(gelenModel.Id);
            if (servis == null)
                return RedirectToAction("TeknisyeneServisKaydiGonder");
            servis.TeknisyenTCNo = gelenModel.Teknisyen.TcNo;
            await repoServisKaydi.UpdateAsync();
            ViewBag.Teknisyen = servis.Teknisyen.ApplicationUser.UserName;
            ViewBag.ServisNumarasi = servis.ServisNumarasi;
            return View();
        }
    }

}