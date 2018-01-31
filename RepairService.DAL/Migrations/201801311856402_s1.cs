namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anketler",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnketBaslik = c.String(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnketSorulari",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnketID = c.Int(nullable: false),
                        SoruMetni = c.String(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Anketler", t => t.AnketID, cascadeDelete: true)
                .Index(t => t.AnketID);
            
            CreateTable(
                "dbo.AnketSorusununCevaplari",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoruID = c.Int(nullable: false),
                        CevapMetni = c.String(),
                        CevapPuani = c.Byte(nullable: false),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnketSorulari", t => t.SoruID, cascadeDelete: true)
                .Index(t => t.SoruID);
            
            CreateTable(
                "dbo.AnketMusteriler",
                c => new
                    {
                        Id1 = c.Int(nullable: false),
                        Id2 = c.String(nullable: false, maxLength: 128),
                        AnketeKatilmaTarihi = c.DateTime(nullable: false),
                        AnketSonucPuani = c.Int(nullable: false),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id1, t.Id2 })
                .ForeignKey("dbo.Anketler", t => t.Id1, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Id2, cascadeDelete: true)
                .Index(t => t.Id1)
                .Index(t => t.Id2);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 25),
                        Surname = c.String(maxLength: 25),
                        RegisterDate = c.DateTime(nullable: false),
                        ActivationCode = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MessageDate = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        SendBy = c.String(nullable: false, maxLength: 128),
                        SentTo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SendBy, cascadeDelete: true)
                .Index(t => t.SendBy);
            
            CreateTable(
                "dbo.Musteriler",
                c => new
                    {
                        TcNo = c.String(nullable: false, maxLength: 11),
                        UserID = c.String(maxLength: 128),
                        TelNo = c.String(maxLength: 10),
                        SistemdekiSonAktifTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TcNo)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ServisKayitlari",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MusteriTCNo = c.String(maxLength: 11),
                        OperatorTCNo = c.String(maxLength: 11),
                        TeknisyenTCNo = c.String(maxLength: 11),
                        CihazModelId = c.Int(nullable: false),
                        ArizaTuruId = c.Int(nullable: false),
                        FaturaId = c.Int(),
                        ServisNumarasi = c.String(),
                        Telefon = c.String(maxLength: 10),
                        AcikAdres = c.String(),
                        MusteriArizaTanimi = c.String(),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MusteriUcretiOnayladiMi = c.Boolean(nullable: false),
                        Durumu = c.Int(nullable: false),
                        KonumLat = c.String(),
                        KonumLng = c.String(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArizaTurleri", t => t.ArizaTuruId, cascadeDelete: true)
                .ForeignKey("dbo.CihazModelleri", t => t.CihazModelId, cascadeDelete: true)
                .ForeignKey("dbo.Musteriler", t => t.MusteriTCNo)
                .ForeignKey("dbo.Operatorler", t => t.OperatorTCNo)
                .ForeignKey("dbo.Teknisyenler", t => t.TeknisyenTCNo)
                .Index(t => t.MusteriTCNo)
                .Index(t => t.OperatorTCNo)
                .Index(t => t.TeknisyenTCNo)
                .Index(t => t.CihazModelId)
                .Index(t => t.ArizaTuruId);
            
            CreateTable(
                "dbo.ServisKaydiIslemleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Aciklama = c.String(),
                        ServisId = c.Int(nullable: false),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServisKayitlari", t => t.ServisId, cascadeDelete: true)
                .Index(t => t.ServisId);
            
            CreateTable(
                "dbo.ArizaTurleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TurAdi = c.Int(nullable: false),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CihazModelleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CihazTuruId = c.Int(nullable: false),
                        MarkaAdi = c.String(),
                        ModelAdi = c.String(maxLength: 50),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CİhazTurleri", t => t.CihazTuruId, cascadeDelete: true)
                .Index(t => t.CihazTuruId);
            
            CreateTable(
                "dbo.CİhazTurleri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tur = c.String(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dosyalar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DosyaYolu = c.String(),
                        Uzanti = c.String(),
                        arizaId = c.Int(),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServisKayitlari", t => t.arizaId)
                .Index(t => t.arizaId);
            
            CreateTable(
                "dbo.Faturalar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServisID = c.Int(nullable: false),
                        Tutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EklenmeTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServisKayitlari", t => t.ServisID, cascadeDelete: true)
                .Index(t => t.ServisID);
            
            CreateTable(
                "dbo.Operatorler",
                c => new
                    {
                        TcNo = c.String(nullable: false, maxLength: 11),
                        UserID = c.String(maxLength: 128),
                        TelNo = c.String(maxLength: 10),
                        SistemdekiSonAktifTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TcNo)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Teknisyenler",
                c => new
                    {
                        TcNo = c.String(nullable: false, maxLength: 11),
                        userID = c.String(maxLength: 128),
                        TelNo = c.String(maxLength: 10),
                        SistemdekiSonAktifTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TcNo)
                .ForeignKey("dbo.AspNetUsers", t => t.userID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.PasifKisiler",
                c => new
                    {
                        TcNo = c.String(nullable: false, maxLength: 11),
                        UserID = c.String(maxLength: 128),
                        TelNo = c.String(maxLength: 10),
                        SistemdekiSonAktifTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TcNo)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 200),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PasifKisiler", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServisKayitlari", "TeknisyenTCNo", "dbo.Teknisyenler");
            DropForeignKey("dbo.Teknisyenler", "userID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServisKayitlari", "OperatorTCNo", "dbo.Operatorler");
            DropForeignKey("dbo.Operatorler", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServisKayitlari", "MusteriTCNo", "dbo.Musteriler");
            DropForeignKey("dbo.Faturalar", "ServisID", "dbo.ServisKayitlari");
            DropForeignKey("dbo.Dosyalar", "arizaId", "dbo.ServisKayitlari");
            DropForeignKey("dbo.CihazModelleri", "CihazTuruId", "dbo.CİhazTurleri");
            DropForeignKey("dbo.ServisKayitlari", "CihazModelId", "dbo.CihazModelleri");
            DropForeignKey("dbo.ServisKayitlari", "ArizaTuruId", "dbo.ArizaTurleri");
            DropForeignKey("dbo.ServisKaydiIslemleri", "ServisId", "dbo.ServisKayitlari");
            DropForeignKey("dbo.Musteriler", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SendBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnketMusteriler", "Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnketMusteriler", "Id1", "dbo.Anketler");
            DropForeignKey("dbo.AnketSorusununCevaplari", "SoruID", "dbo.AnketSorulari");
            DropForeignKey("dbo.AnketSorulari", "AnketID", "dbo.Anketler");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.PasifKisiler", new[] { "UserID" });
            DropIndex("dbo.Teknisyenler", new[] { "userID" });
            DropIndex("dbo.Operatorler", new[] { "UserID" });
            DropIndex("dbo.Faturalar", new[] { "ServisID" });
            DropIndex("dbo.Dosyalar", new[] { "arizaId" });
            DropIndex("dbo.CihazModelleri", new[] { "CihazTuruId" });
            DropIndex("dbo.ServisKaydiIslemleri", new[] { "ServisId" });
            DropIndex("dbo.ServisKayitlari", new[] { "ArizaTuruId" });
            DropIndex("dbo.ServisKayitlari", new[] { "CihazModelId" });
            DropIndex("dbo.ServisKayitlari", new[] { "TeknisyenTCNo" });
            DropIndex("dbo.ServisKayitlari", new[] { "OperatorTCNo" });
            DropIndex("dbo.ServisKayitlari", new[] { "MusteriTCNo" });
            DropIndex("dbo.Musteriler", new[] { "UserID" });
            DropIndex("dbo.Messages", new[] { "SendBy" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AnketMusteriler", new[] { "Id2" });
            DropIndex("dbo.AnketMusteriler", new[] { "Id1" });
            DropIndex("dbo.AnketSorusununCevaplari", new[] { "SoruID" });
            DropIndex("dbo.AnketSorulari", new[] { "AnketID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.PasifKisiler");
            DropTable("dbo.Teknisyenler");
            DropTable("dbo.Operatorler");
            DropTable("dbo.Faturalar");
            DropTable("dbo.Dosyalar");
            DropTable("dbo.CİhazTurleri");
            DropTable("dbo.CihazModelleri");
            DropTable("dbo.ArizaTurleri");
            DropTable("dbo.ServisKaydiIslemleri");
            DropTable("dbo.ServisKayitlari");
            DropTable("dbo.Musteriler");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AnketMusteriler");
            DropTable("dbo.AnketSorusununCevaplari");
            DropTable("dbo.AnketSorulari");
            DropTable("dbo.Anketler");
        }
    }
}
