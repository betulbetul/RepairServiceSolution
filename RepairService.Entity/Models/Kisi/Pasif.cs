using RepairService.Entity.IdentityModels;
using RepairService.Entity.Kisi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models.Kisi
{
    [Table("PasifKisiler")]
    public class Pasif: KisiTemel
    {
        public string UserID { get; set; } //Identity Modelsdeki ID burada FK olacak.

        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    
    }
}
