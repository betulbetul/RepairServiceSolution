using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.BLL.Settings;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RepairService.UI.MVC.Controllers
{


    public class BaseController : Controller
    {
        [NonAction]
        public void ResimBoyutlandir(int en, int boy, string yol)
        {
            WebImage img = new WebImage(yol);
            img.Resize(en, boy, false);
            img.AddTextWatermark("SmartTV Service", fontColor: "Tomato", fontSize: 18, fontFamily: "Verdana");
            img.Save(yol);
        }
        //[NonAction]
        //public async Task<List<SelectListItem>> XxxSelectList()
        //{
        //    //var arizaKaydiList = await new ArizaKaydiRepo().GetAll();
        //    //var kategoriler = new List<SelectListItem>();
        //    //kategoriler.Add(new SelectListItem()
        //    //{
        //    //    Text = "Üst Kategorisi Yok",
        //    //    Value = "0"
        //    //});
        //    //kategoriList.ForEach(x =>
        //    //kategoriler.Add(new SelectListItem
        //    //{
        //    //    Text = x.KategoriAdi,
        //    //    Value = x.Id.ToString()
        //    //})
        //    //);
        //    //return kategoriler;
        //    return null;

        //}

        [NonAction]
        public string RandomYeniSifreOlustur()
        {
            //abcd1234
            Random rnd = new Random();
            int sayi = rnd.Next(1000, 5000);
            char[] metin = Guid.NewGuid().ToString().Replace("-", "").ToArray();
            string sifre = string.Empty;
            for (int i = 0; i < metin.Length; i++)
            {
                if (sifre.Length == 4) break;
                if (char.IsLetter(metin[i]))
                    sifre += metin[i].ToString();
            }
            sifre += sayi;
            return sifre;
        }

        public string RandomServisKaydıNumarasiOlustur()
        {
            //SRVS...
            Random rnd = new Random();
            int sayi = rnd.Next(100000, 999999);
            return $"SRVS" + sayi;
        }

        [NonAction]
        public async Task AnketMailiGonder(int servisID, int anketID) //Müşteri kim servisten bulunur
        {
            //servis
            ServisKaydi servis = new ServisKaydiRepo().GetById(servisID);
            //Anket 
            Anket anket = new AnketRepo().GetById(anketID);
            //mail
            string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
   (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            await SiteSettings.SendMail(new MailModel()
            {
                To = servis.Musteri.ApplicationUser.Email,
                Subject = $"Smart TV Teknik Servis - {servis.ServisNumarasi} ile ilgili {anket.AnketBaslik}",
                Message=$"Sayın {servis.Musteri.ApplicationUser.Name+ " "+ servis.Musteri.ApplicationUser.Surname}, <br /> {servis.EklenmeTarihi + " tarihindeki " + "ServisNO: " + servis.ServisNumarasi } servisiniz çözüme ulaşmıştır. <br /> Servis ile ilgili bir ankete katılıp değerlendirmek için <b><a href='{siteUrl}/Anket/AnketeKatil?servisID={servisID}&anketID={anketID}'>BURAYA</a></b> tıklayınız."

            });
        }
    }

}