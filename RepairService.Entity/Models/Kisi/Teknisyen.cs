﻿using RepairService.Entity.IdentityModels;
using RepairService.Entity.Models.Cihaz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Kisi
{

    [Table("Teknisyenler")]

    public class Teknisyen : KisiTemel
    {
        public string userID { get; set; } //Identity Modelsdeki ID burada FK olacak.

        [ForeignKey("userID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<ServisKaydi> ServisKaydiList { get; set; } = new List<ServisKaydi>();

    }

}