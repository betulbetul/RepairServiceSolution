using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{
    [Table("AnketSorusununCevaplari")]
    public class AnketSorusununCevap : Temel<int>
    {
        public int SoruID { get; set; }
        public string CevapMetni { get; set; }
        public byte CevapPuani { get; set; }

        [ForeignKey("SoruID")]
        public virtual AnketSoru AnketSoru { get; set; }
    }
}
