using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{
    [Table("AnketSorulari")]
    public class AnketSoru
    {
        [Key]
        [Column(Order = 1)]
        public int id { get; set; }
        public int AnketID { get; set; } //FK
        public string SoruMetni { get; set; }

        //Virtual bağlı olduğu anket
        [ForeignKey("AnketID")]
        public virtual Anket Anket { get; set; }
        public virtual List<AnketSorusununCevap> AnketSorusununCevapList { get; set; } = new List<AnketSorusununCevap>();
    }
}
