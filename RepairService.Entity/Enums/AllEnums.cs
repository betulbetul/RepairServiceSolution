using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.Enums
{
    public enum IdentityRoles : byte
    {
        Admin = 0,
        Operator = 1,
        TecnicalPerson = 2,
        Customer = 3,
        Passive = 4,
        Active = 5
    }

    public enum ArizaDurum
    {
        Onay_Bekliyor = 0, //Yeni arıza kaydı açılmış
        Operatore_Aktarildi = 1, // Operator üzerine aldı.
        Teknisyene_Aktarildi = 2, // Teknisyen üzerine aldı ve inceliyor.
        ParcaBekliyor = 3, // parça bekliyor
        MusteriOnayiBekleniyor = 5, // Müşteriye sunulan para teklifini müşteri kabul ediyor mu?
        Musteri_Onayladi = 6,
        TeknisyenServiseCikti = 7, // Teknisyen yerinde servise gitti.        
        Cozuldu = 8,// Arıza çözümlendi
        Iptal_Edildi = 9 //Arıza İptal edildi.

    }

    public enum ArizaTurleri
    {
        Yazilimsal = 0,
        Donanimsal = 1
    }


}


