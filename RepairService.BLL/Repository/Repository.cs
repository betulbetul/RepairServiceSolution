﻿using RepairService.Entity.Kisi;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using RepairService.Entity.Models.Kisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.BLL.Repository
{

    public class MessageRepo : RepositoryBase<Message, long> { }
    public class MusteriRepo : RepositoryBase<Musteri, string> { }
    public class OperatorRepo : RepositoryBase<Operator, string> { }
    public class TeknisyenRepo : RepositoryBase<Teknisyen, string> { }
    public class PasifRepo : RepositoryBase<Pasif, string> { }
    public class ServisKaydiRepo : RepositoryBase<ServisKaydi, int> { }
    public class ServisKaydiIslemRepo : RepositoryBase<ServisKaydiIslem, int> { }
    public class CihazTuruRepo : RepositoryBase<CihazTuru, int> { }
    public class CihazModelRepo : RepositoryBase<CihazModel, int> { }
    public class ArizaTuruRepo : RepositoryBase<ArizaTuru, int> { }
    public class DosyaRepo : RepositoryBase<Dosya, long> { }
    public class FaturaRepo : RepositoryBase<Fatura, int> { }

    public class AnketRepo : RepositoryBase<Anket, int> { }
    public class AnketSoruRepo : RepositoryBase<AnketSoru, int> { }
    public class AnketSorusununCevapRepo : RepositoryBase<AnketSorusununCevap, int> { }
    public class AnketMusteriRepo : RepositoryBase<AnketMusteri, int> { }
}
