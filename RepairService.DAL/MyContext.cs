using Microsoft.AspNet.Identity.EntityFramework;
using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.Models.Kisi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.DAL
{

    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext()
            : base("name=MyCon")
        { }

        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Musteri> Musteriler { get; set; }
        public virtual DbSet<Operator> Operatorler { get; set; }
        public virtual DbSet<Teknisyen> Teknisyenler { get; set; }
        public virtual DbSet<Pasif> PasifKisiler { get; set; }
        public virtual DbSet<ServisKaydi> ServisKayitlari { get; set; }
        public virtual DbSet<CihazTuru> CihazTurleri { get; set; }
        public virtual DbSet<CihazModel> CihazModelleri { get; set; }
        public virtual DbSet<ArizaTuru> ArizaTurleri { get; set; }
        public virtual DbSet<Dosya> Dosyalar { get; set; }
        public virtual DbSet<ServisKaydiIslem> ServisKaydiIslemleri { get; set; }
        public virtual DbSet<Fatura> Faturalar { get; set; }

    }


}
