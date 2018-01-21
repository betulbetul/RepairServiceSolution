using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.ViewModels
{
    public class MailModel
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class IletisimMailModel
    {
        public string IsimSoyisim { get; set; }
        public string EmailAdresi { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Konu { get; set; } = "SMART TV REPAIR SERVICE | MÜŞTERİ İLETİŞİM TALEP FORMU";
        public string Mesaj { get; set; }
    }


}
