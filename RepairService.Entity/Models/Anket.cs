using RepairService.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{
    [Table("Anketler")]
    public class Anket : Temel<int>
    {
        public string AnketBaslik { get; set; }
        //Virtual Soru Listesi
        public virtual List<AnketSoru> AnketSoruList { get; set; }
    }
}
