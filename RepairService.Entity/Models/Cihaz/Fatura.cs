using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{
    [Table("Faturalar")]
    public class Fatura : Temel<int>
    {
        public int ServisID { get; set; }
        [Required]
        public decimal Tutar { get; set; } = 0m;
        [ForeignKey("ServisID")]
        public virtual ServisKaydi ServisKaydi { get; set; }
    }
}
