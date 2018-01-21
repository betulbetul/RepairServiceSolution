using RepairService.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Models
{

    [Table("Messages")]
    public class Message
    {
        [Key]
        public long Id { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.Now;
        [Required]
        public string Content { get; set; }
        [Required]
        public string SendBy { get; set; }
        [Required]
        public string SentTo { get; set; }


        [ForeignKey("SendBy")]
        public virtual ApplicationUser Sender { get; set; }
    }

}
