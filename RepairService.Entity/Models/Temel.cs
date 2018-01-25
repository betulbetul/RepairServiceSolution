using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{

    public class Temel<T> : ITemel
    {
        [Key]
        [Column(Order = 1)]
        public T Id { get; set; }
        //[Display(Name = "Aktif Mi")]
        //public bool AktifMi { get; set; } = true;
        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

    }

}
