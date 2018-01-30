namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankt2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnketSorulari", "EklenmeTarihi", c => c.DateTime(nullable: false));
            AddColumn("dbo.AnketSorusununCevaplari", "EklenmeTarihi", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnketSorusununCevaplari", "EklenmeTarihi");
            DropColumn("dbo.AnketSorulari", "EklenmeTarihi");
        }
    }
}
