namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b1 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PasifKisiler", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.PasifKisiler", new[] { "UserID" });
            DropTable("dbo.PasifKisiler");
        }
    }
}
