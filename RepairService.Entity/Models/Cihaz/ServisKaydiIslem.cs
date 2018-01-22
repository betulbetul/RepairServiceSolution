using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{
    [Table("ServisKaydiIslemleri")]
    public class ServisKaydiIslem : Temel<int> // Kendisi ve ArizaKaydi
    {
        public string Aciklama { get; set; }
        public int ServisId { get; set; }

        [ForeignKey("ServisId")]
        public virtual ServisKaydi ServisKaydi { get; set; }
    }

}
