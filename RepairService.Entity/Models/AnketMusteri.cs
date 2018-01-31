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
    [Table("AnketMusteriler")]
    public class AnketMusteri : AraTemel<int, string>
    {
        [Index(IsUnique =true)]
        public int servisID { get; set; } //FK DEĞİL!! Unique ile tekrar etmesi önlenecek!
        public DateTime AnketeKatilmaTarihi { get; set; }
        public int AnketSonucPuani { get; set; }
        [ForeignKey("Id1")]
        public virtual Anket Anket { get; set; }
        [ForeignKey("Id2")]
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}
