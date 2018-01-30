﻿using RepairService.Entity.Models;
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

        public ActionResult AnketEkle()
        {
            Anket model = new Anket();
            return View(model);
        }
        [HttpPost]
        public ActionResult AnketEkle(Anket model)
        {
            return View(model);
        }

    }
}