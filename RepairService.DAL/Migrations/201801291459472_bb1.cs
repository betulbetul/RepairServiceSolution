namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bb1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServisKayitlari", "FaturaId", "dbo.Faturalar");
            DropIndex("dbo.ServisKayitlari", new[] { "FaturaId" });
            CreateIndex("dbo.Faturalar", "ServisID");
            AddForeignKey("dbo.Faturalar", "ServisID", "dbo.ServisKayitlari", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Faturalar", "ServisID", "dbo.ServisKayitlari");
            DropIndex("dbo.Faturalar", new[] { "ServisID" });
            CreateIndex("dbo.ServisKayitlari", "FaturaId");
            AddForeignKey("dbo.ServisKayitlari", "FaturaId", "dbo.Faturalar", "Id");
        }
    }
}
