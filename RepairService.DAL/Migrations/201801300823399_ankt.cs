namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnketSorusununCevaplari", new[] { "AnketSoru_id", "AnketSoru_AnketID" }, "dbo.AnketSorulari");
            DropIndex("dbo.AnketSorusununCevaplari", new[] { "AnketSoru_id", "AnketSoru_AnketID" });
            RenameColumn(table: "dbo.AnketSorusununCevaplari", name: "AnketSoru_id", newName: "SoruID");
            DropPrimaryKey("dbo.AnketSorulari");
            AlterColumn("dbo.AnketSorulari", "id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AnketSorusununCevaplari", "SoruID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AnketSorulari", "id");
            CreateIndex("dbo.AnketSorusununCevaplari", "SoruID");
            AddForeignKey("dbo.AnketSorusununCevaplari", "SoruID", "dbo.AnketSorulari", "id", cascadeDelete: true);
            DropColumn("dbo.AnketSorusununCevaplari", "AnketSoru_AnketID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnketSorusununCevaplari", "AnketSoru_AnketID", c => c.Int());
            DropForeignKey("dbo.AnketSorusununCevaplari", "SoruID", "dbo.AnketSorulari");
            DropIndex("dbo.AnketSorusununCevaplari", new[] { "SoruID" });
            DropPrimaryKey("dbo.AnketSorulari");
            AlterColumn("dbo.AnketSorusununCevaplari", "SoruID", c => c.Int());
            AlterColumn("dbo.AnketSorulari", "id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AnketSorulari", new[] { "id", "AnketID" });
            RenameColumn(table: "dbo.AnketSorusununCevaplari", name: "SoruID", newName: "AnketSoru_id");
            CreateIndex("dbo.AnketSorusununCevaplari", new[] { "AnketSoru_id", "AnketSoru_AnketID" });
            AddForeignKey("dbo.AnketSorusununCevaplari", new[] { "AnketSoru_id", "AnketSoru_AnketID" }, "dbo.AnketSorulari", new[] { "id", "AnketID" });
        }
    }
}
