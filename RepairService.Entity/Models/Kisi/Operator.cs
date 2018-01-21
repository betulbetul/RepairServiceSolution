using RepairService.Entity.IdentityModels;
using RepairService.Entity.Models.Cihaz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Kisi
{
    [Table("Operatorler")]

    public class Operator : KisiTemel
    {
        public string UserID { get; set; } //Identity Modelsdeki ID burada FK olacak.

        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Arizakayitları olacak...
        public virtual List<ServisKaydi> ServisKaydiList { get; set; } = new List<ServisKaydi>();
    }

}
