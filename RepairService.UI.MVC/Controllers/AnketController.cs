using RepairService.BLL.Repository;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.ViewModels;
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
            ViewBag.Anketler = new AnketRepo().GetAll().ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult AnketEkle(Anket model)
        {
            ViewBag.Anketler = new AnketRepo().GetAll().ToList();
            return View(model);
        }

        //Müşteri mailden tıklayıp ankete katılacak!
        public ActionResult AnketeKatil(int servisID, int anketID)
        { //ANKETVIEWMODEL dolacak ve View'e gidecek! 
            //servis
            ServisKaydi servis = new ServisKaydiRepo().GetById(servisID);
            //Anket 
            Anket anket = new AnketRepo().GetById(anketID);
            //mail
            AnketViewModel model = new AnketViewModel()
            {
                AnketBaslik = anket.AnketBaslik,
                AnketID = anket.Id,
                Servis = servis,
                Musteri = servis.Musteri
            };
            anket.AnketSoruList.ForEach(x => model.Sorular.Add(new AnketSoru()
            {
                AnketID = x.AnketID,
                Id = x.Id,
                SoruMetni = x.SoruMetni,
                AnketSorusununCevapList = x.AnketSorusununCevapList
            }));
            return View(model);

        }
        [HttpPost]
        public ActionResult AnketeKatil()
        {

            return View();
        }

       

    }
}