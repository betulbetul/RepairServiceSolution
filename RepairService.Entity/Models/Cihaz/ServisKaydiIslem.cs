using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{
    [Table("ServisKaydiIslemleri")]
    public class ServisKaydiIslem : AraTemel<int, int> // Kendisi ve ArizaKaydi
    {
        public string Aciklama { get; set; }

        [ForeignKey("Id2")]
        public virtual ServisKaydi ServisKaydi { get; set; }
    }

}
