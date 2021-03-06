﻿using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.BLL.Repository;
using RepairService.Entity.Enums;
using RepairService.Entity.IdentityModels;
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
    // Sadece Teknisyen erişebilir. // 
    [Authorize(Roles = "TecnicalPerson")]
    public class TeknisyenController : BaseController
    {
        const int pageSize = 24;
        // GET: Teknisyen
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

        [HttpGet]
        public ActionResult TeknisyeninServisleri()
        {
            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(HttpContext.User.Identity.GetUserId()) ?? null;
            var teknisyenSonuc = new TeknisyenRepo().GetAll().FirstOrDefault(x => x.userID == user.Id);
            // Teknisyene ait tüm servisler
            var teknisyenServisler = new ServisKaydiRepo().GetAll().Where(x => x.TeknisyenTCNo == teknisyenSonuc.TcNo).ToList();
            return View(teknisyenServisler); // Teknisyene ait tüm servisler

        }

        public ActionResult TeknisyenServisDetayIslemi(int? id)
        {
            if (id == null)
                return RedirectToAction("TeknisyeninServisleri");
            var servisKaydi = new ServisKaydiRepo().GetAll().FirstOrDefault(x => x.Id == id);
            ServisKaydiViewModel model = new ServisKaydiViewModel()
            {
                ServisId = servisKaydi.Id,
                ArizaTurAdi = servisKaydi.ArizaTuru.TurAdi.ToString(),
                CihazMarka = servisKaydi.CihazModel.MarkaAdi,
                CihazModel = servisKaydi.CihazModel.ModelAdi,
                CihazTuru = servisKaydi.CihazModel.CihazTuru.Tur,
                musteriArizaTanimi = servisKaydi.MusteriArizaTanimi,
                MusteriUcretiOnayladiMi = servisKaydi.MusteriUcretiOnayladiMi,
                Durumu = servisKaydi.Durumu,
                TeknisyenUserID = servisKaydi.Teknisyen.userID,
                OperatorUserID = servisKaydi.Operator.UserID,
                MusteriUserID = servisKaydi.Musteri.UserID,
                Fiyat = servisKaydi.Fiyat,
                EklenmeTarihi = servisKaydi.EklenmeTarihi,
                KonumLat = servisKaydi.KonumLat,
                KonumLng = servisKaydi.KonumLng,
                TeknisyenAciklamasi = string.Empty,
                ServisNumarasi = servisKaydi.ServisNumarasi,
                AcikAdres = servisKaydi.AcikAdres,
                Telefon = servisKaydi.Telefon
                //fotoğraflar
            };
            var dosyalar = new DosyaRepo().GetAll().Where(x => x.arizaId == servisKaydi.Id).ToList();
            dosyalar.ForEach(x => model.FotoUrList.Add($"{x.DosyaYolu}"));
            var aciklamalar = new ServisKaydiIslemRepo().GetAll().Where(x => x.ServisId == id).ToList();
            ViewBag.Aciklamalar = aciklamalar;
            //Enum
            var durumList = Enum.GetValues(typeof(ArizaDurum)).Cast<ArizaDurum>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).Where(x => Convert.ToInt32(x.Value) >= Convert.ToInt32(servisKaydi.Durumu)).ToList();


            ViewBag.DurumList = durumList;
            if (servisKaydi.FaturaList.Count > 0)
                model.Fatura = servisKaydi.FaturaList.Where(x => x.ServisID == model.ServisId).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeknisyenServisDetayIslemi(ServisKaydiViewModel gelenModel)
        {
            ServisKaydiRepo repoServisKaydi = new ServisKaydiRepo();
            var servisKaydi = repoServisKaydi.GetAll().Where(x => gelenModel.ServisId == x.Id).FirstOrDefault();
            servisKaydi.Fiyat = gelenModel.Fiyat;
            servisKaydi.Durumu = gelenModel.Durumu;
            repoServisKaydi.Update();
            if (servisKaydi.Fiyat > 0 && servisKaydi.Durumu == ArizaDurum.Teknisyene_Aktarildi)
            {
                servisKaydi.Durumu = Entity.Enums.ArizaDurum.MusteriOnayiBekleniyor;
                repoServisKaydi.Update();
            }
            if (gelenModel.TeknisyenAciklamasi != null)
            {
                //işlemi ilişkili tabloya kaydet
                ServisKaydiIslem islem = new ServisKaydiIslem();
                islem.ServisId = servisKaydi.Id;
                islem.Aciklama = gelenModel.TeknisyenAciklamasi;
                islem.EklenmeTarihi = DateTime.Now;
                new ServisKaydiIslemRepo().Insert(islem);
            }

            ViewBag.servisNumarasi = servisKaydi.ServisNumarasi;
            //Modeli doldur ve geri gönder
            ServisKaydiViewModel model = new ServisKaydiViewModel()
            {
                ServisId = servisKaydi.Id,
                ArizaTurAdi = servisKaydi.ArizaTuru.TurAdi.ToString(),
                CihazMarka = servisKaydi.CihazModel.MarkaAdi,
                CihazModel = servisKaydi.CihazModel.ModelAdi,
                CihazTuru = servisKaydi.CihazModel.CihazTuru.Tur,
                musteriArizaTanimi = servisKaydi.MusteriArizaTanimi,
                MusteriUcretiOnayladiMi = servisKaydi.MusteriUcretiOnayladiMi,
                Durumu = servisKaydi.Durumu,
                TeknisyenUserID = servisKaydi.Teknisyen.userID,
                OperatorUserID = servisKaydi.Operator.UserID,
                MusteriUserID = servisKaydi.Musteri.UserID,
                Fiyat = servisKaydi.Fiyat,
                EklenmeTarihi = servisKaydi.EklenmeTarihi,
                KonumLat = servisKaydi.KonumLat,
                KonumLng = servisKaydi.KonumLng,
                TeknisyenAciklamasi = string.Empty,
                ServisNumarasi = servisKaydi.ServisNumarasi,
                AcikAdres = servisKaydi.AcikAdres,
                Telefon = servisKaydi.Telefon
                //fotoğraflar
            };
            var dosyalar = new DosyaRepo().GetAll().Where(x => x.arizaId == servisKaydi.Id).ToList();
            dosyalar.ForEach(x => model.FotoUrList.Add($"{x.DosyaYolu}"));
            var aciklamalar = new ServisKaydiIslemRepo().GetAll().Where(x => x.ServisId == gelenModel.ServisId).ToList();
            ViewBag.Aciklamalar = aciklamalar;
            //Enum
            var durumList = Enum.GetValues(typeof(ArizaDurum)).Cast<ArizaDurum>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).Where(x => Convert.ToInt32(x.Value) >= Convert.ToInt32(servisKaydi.Durumu)).ToList();

            ViewBag.DurumList = durumList;
            //Eğer arıza çözüldü ya da iptal edildiyse ANKET GÖNDER
            if (servisKaydi.Durumu == ArizaDurum.Cozuldu || servisKaydi.Durumu == ArizaDurum.Iptal_Edildi)
            {
                await AnketMailiGonder(servisKaydi.Id, 1);
            }
            return View(model);
        }

        public ActionResult AciklamaSil(int? islemID)
        {
            if (islemID == null || islemID == 0)
                return RedirectToAction("TeknisyenServisDetayIslemi");
            ServisKaydiIslemRepo repoServisIslem = new ServisKaydiIslemRepo();
            var servisDetayIslem = repoServisIslem.GetAll().FirstOrDefault(x => x.Id == islemID);
            repoServisIslem.Delete(servisDetayIslem);
            //Fatura Oluşacak!
            return RedirectToAction("TeknisyenServisDetayIslemi", new { @class = servisDetayIslem.ServisId });
        }
    }

}