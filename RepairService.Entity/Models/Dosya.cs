using RepairService.Entity.Models.Cihaz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{

    [Table("Dosyalar")]
    public class Dosya : Temel<long>
    {
        public string DosyaYolu { get; set; }
        public string Uzanti { get; set; }
        public int? arizaId { get; set; }

        [ForeignKey("arizaId")]
        public virtual ServisKaydi ServisKaydi { get; set; }
    }

}
