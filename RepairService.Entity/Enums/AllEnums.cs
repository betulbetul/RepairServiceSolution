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
        onayBekliyor = 0, //Yeni arıza kaydı açılmış
        operatoreAktarildi = 1, // Operator üzerine aldı.
        teknisyeneAktarildi = 2, // Teknisyen üzerine aldı ve inceliyor.
        parcaBekliyor = 3, // parça bekliyor
        teknisyenServiseCikti = 4, // Teknisyen yerinde servise gitti.
        musteriOnayiBekleniyor = 5, // Müşteriye sunulan para teklifini müşteri kabul ediyor mu?
        Cozuldu = 6// Arıza çözümlendi
    }

    public enum ArizaTurleri
    {
        Yazilimsal = 0,
        Donanimsal = 1
    }


}


