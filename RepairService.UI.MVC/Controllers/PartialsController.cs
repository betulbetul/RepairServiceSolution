using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{

    public class PartialsController : Controller
    {
        const int pageSize = 24;

        public PartialViewResult preHeaderResult()
        {
            return PartialView("_PartialPagePreHeader");
        }

        public PartialViewResult backgroundResult()
        {
            return PartialView("_PartialPageBackground");
        }

        public PartialViewResult headerResult()
        {
            return PartialView("_PartialPageHeader");
        }
        public PartialViewResult topMenuResult()
        {
            return PartialView("_PartialPageTopMenu");
        }
        public PartialViewResult footerResult()
        {
            return PartialView("_PartialPageFooter");
        }
        public PartialViewResult adminSideBarMenuResult()
        {
            var servisKayitlari = new ServisKaydiRepo().GetAll().ToList();
            return PartialView("_PartialPageAdminSideBarMenu", servisKayitlari);

        }

        public PartialViewResult operatorSideBarMenuResult()
        {
            var servisKayitlari = new ServisKaydiRepo().GetAll().ToList();
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            TempData["Operator"] = user.UserName;
            TempData["OperatorTCNo"] = user.OperatorList.FirstOrDefault().TcNo;
            return PartialView("_PartialPageOperatorSideBarMenu", servisKayitlari);

        }

        public PartialViewResult teknisyenSideBarMenuResult()
        {
            var servisKayitlari = new ServisKaydiRepo().GetAll().ToList();
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
            TempData["Teknisyen"] = user.UserName;
            TempData["TeknisyenTCNo"] = user.TeknisyenList.FirstOrDefault().TcNo;
            return PartialView("_PartialPageTeknisyenSideBarMenu", servisKayitlari);

        }

    }



}