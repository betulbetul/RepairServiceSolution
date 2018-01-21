using RepairService.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Kisi
{

    public abstract class KisiTemel : IKisi
    {
        [Key]
        [Column(Order = 1)]
        [MinLength(11)]
        [StringLength(11, ErrorMessage = "Tc kimlik numarası 11 haneli olmalıdır!")]
        public string TcNo { get; set; }
        [MinLength(10)]
        [StringLength(10, ErrorMessage = "Telefon numarasını başında sıfır olmadan, 10 haneli şekilde giriniz.")]
        public string TelNo { get; set; }
        public DateTime SistemdekiSonAktifTarihi { get; set; } = DateTime.Now;

    }

}
