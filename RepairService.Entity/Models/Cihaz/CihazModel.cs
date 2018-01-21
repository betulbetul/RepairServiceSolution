using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{
    [Table("CihazModelleri")]
    public class CihazModel : Temel<int>
    {
        public int CihazTuruId { get; set; }
        public string MarkaAdi { get; set; }

        [StringLength(50)]
        public string ModelAdi { get; set; }

        [ForeignKey("CihazTuruId")]
        public virtual CihazTuru CihazTuru { get; set; }

        public virtual List<ServisKaydi> ArizaKaydiList { get; set; } = new List<ServisKaydi>();

    }

}
