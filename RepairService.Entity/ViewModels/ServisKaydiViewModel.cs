using Microsoft.AspNet.Identity;
using RepairService.Entity.Enums;
using RepairService.Entity.Models.Cihaz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RepairService.Entity.ViewModels
{
    public class ServisKaydiViewModel
    {
        public int ServisId { get; set; }
        public string MusteriUserID { get; set; }
        public string OperatorUserID { get; set; }
        public string TeknisyenUserID { get; set; }
        public string ServisNumarasi { get; set; }

        [StringLength(10, ErrorMessage = "Telefon numaranızı başında sıfır olmadan 10 haneli olacak şekilde yazınız.")] //216 666 66 66 // 536 666 66 66
        public string Telefon { get; set; }
        public string AcikAdres { get; set; }
        public string CihazTuru { get; set; } //Tablodan gelecek (Smart Tv ya da kumanda)  

        public string CihazMarka { get; set; } //Müşteri kendisi yazacak 

        public string CihazModel { get; set; } // Müşteri kendisi yazacak 

        public string ArizaTurAdi { get; set; } //Tablodan gelecek (Yazılımsal ya da Donanimsal) 
        [MinLength(5, ErrorMessage = "Arıza kaydı ile ilgili tanım yazmalısınız!")]
        public string musteriArizaTanimi { get; set; } //Müşteri kendisi yazacak
        public decimal Fiyat { get; set; } = 0m;
        public bool MusteriUcretiOnayladiMi { get; set; } = false;
        public ArizaDurum Durumu { get; set; }
        public List<string> FotoUrList { get; set; } = new List<string>();
        public List<HttpPostedFileBase> Dosyalar { get; set; } = new List<HttpPostedFileBase>();

        //Konum
        public string KonumLat { get; set; }
        public string KonumLng { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

        public string TeknisyenAciklamasi { get; set; }
        public Fatura Fatura { get; set; }
    }
}
