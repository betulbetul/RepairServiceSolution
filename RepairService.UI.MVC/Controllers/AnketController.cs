using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{
    public class AnketController : BaseController
    {
        // GET: Anket
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AnketGonder()
        {
            return View();
        }
    }
}