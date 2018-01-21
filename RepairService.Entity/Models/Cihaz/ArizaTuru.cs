using RepairService.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{
    [Table("ArizaTurleri")]
    public class ArizaTuru : Temel<int>
    {
        public ArizaTurleri TurAdi { get; set; }

    }

}
