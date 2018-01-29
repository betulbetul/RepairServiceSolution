using RepairService.Entity.Enums;
using RepairService.Entity.Kisi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{

    [Table("ServisKayitlari")]
    public class ServisKaydi : Temel<int>
    {
        public string MusteriTCNo { get; set; } //Ariza kaydini açan müşteri
        public string OperatorTCNo { get; set; } //üzerine alacak olan operator
        public string TeknisyenTCNo { get; set; } //gönderilecek olan  teknisyen
        public int CihazModelId { get; set; }
        public int ArizaTuruId { get; set; }
        public int? FaturaId { get; set; }
        public string ServisNumarasi { get; set; }
        [StringLength(10, ErrorMessage = "Telefon numaranızı başında sıfır olmadan 10 haneli olacak şekilde yazınız.")] //216 666 66 66 // 536 666 66 66
        public string Telefon { get; set; }
        public string AcikAdres { get; set; }

        [MinLength(5, ErrorMessage = "Arıza kaydı ile ilgili açıklama yazmalısınız!")]
        public string MusteriArizaTanimi { get; set; }
        public decimal Fiyat { get; set; } = 0m;
        public bool MusteriUcretiOnayladiMi { get; set; } = false;
        public ArizaDurum Durumu { get; set; }
        //Konum
        public string KonumLat { get; set; }
        public string KonumLng { get; set; }

        [ForeignKey("MusteriTCNo")]
        public virtual Musteri Musteri { get; set; }

        [ForeignKey("OperatorTCNo")]
        public virtual Operator Operator { get; set; }

        [ForeignKey("TeknisyenTCNo")]
        public virtual Teknisyen Teknisyen { get; set; }

        [ForeignKey("CihazModelId")]
        public virtual CihazModel CihazModel { get; set; }

        [ForeignKey("ArizaTuruId")]
        public virtual ArizaTuru ArizaTuru { get; set; }

        public virtual List<ServisKaydiIslem> ArizaKaydiIslemList { get; set; } = new List<ServisKaydiIslem>();
        public virtual List<Dosya> DosyaList { get; set; } = new List<Dosya>();
        public virtual List<Fatura> FaturaList { get; set; } = new List<Fatura>();

    }

}
