using Microsoft.AspNet.Identity.EntityFramework;
using RepairService.Entity.Kisi;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Kisi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.IdentityModels
{

    public class ApplicationUser : IdentityUser
    {
        [StringLength(25)]
        public string Name { get; set; }
        [StringLength(25)]
        public string Surname { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string ActivationCode { get; set; }
        public virtual List<Message> Messages { get; set; } = new List<Message>();

        public virtual List<Musteri> MusteriList { get; set; } = new List<Musteri>();
        public virtual List<Operator> OperatorList { get; set; } = new List<Operator>();
        public virtual List<Teknisyen> TeknisyenList { get; set; } = new List<Teknisyen>();
        public virtual List<Pasif> PasifList { get; set; } = new List<Pasif>();
        public virtual List<Anket> AnketList { get; set; } = new List<Anket>();
    }
}


