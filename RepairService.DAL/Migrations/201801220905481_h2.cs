namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class h2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServisKayitlari", "Telefon", c => c.String(maxLength: 10));
            AddColumn("dbo.ServisKayitlari", "AcikAdres", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServisKayitlari", "AcikAdres");
            DropColumn("dbo.ServisKayitlari", "Telefon");
        }
    }
}
