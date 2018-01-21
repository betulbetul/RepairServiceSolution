using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Cihaz
{

    [Table("CİhazTurleri")]
    public class CihazTuru : Temel<int>
    {
        public string Tur { get; set; }

        public virtual List<CihazModel> CihazModelList { get; set; } = new List<CihazModel>();
    }

}
