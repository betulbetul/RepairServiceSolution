namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnketMusteriler", "servisID", c => c.Int(nullable: false));
            CreateIndex("dbo.AnketMusteriler", "servisID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AnketMusteriler", new[] { "servisID" });
            DropColumn("dbo.AnketMusteriler", "servisID");
        }
    }
}
