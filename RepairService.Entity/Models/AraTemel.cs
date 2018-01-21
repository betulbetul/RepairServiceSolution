using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{

    public class AraTemel<T1, T2> : ITemel
    {
        [Key]
        [Column(Order = 1)]
        public T1 Id1 { get; set; }
        [Key]
        [Column(Order = 2)]
        public T2 Id2 { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }

}
