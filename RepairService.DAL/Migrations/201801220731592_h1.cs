namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class h1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ServisKaydiIslemleri", name: "Id2", newName: "ServisId");
            RenameIndex(table: "dbo.ServisKaydiIslemleri", name: "IX_Id2", newName: "IX_ServisId");
            DropPrimaryKey("dbo.ServisKaydiIslemleri");
            AddColumn("dbo.ServisKaydiIslemleri", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ServisKaydiIslemleri", "Id");
            DropColumn("dbo.ServisKaydiIslemleri", "Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServisKaydiIslemleri", "Id1", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.ServisKaydiIslemleri");
            DropColumn("dbo.ServisKaydiIslemleri", "Id");
            AddPrimaryKey("dbo.ServisKaydiIslemleri", new[] { "Id1", "Id2" });
            RenameIndex(table: "dbo.ServisKaydiIslemleri", name: "IX_ServisId", newName: "IX_Id2");
            RenameColumn(table: "dbo.ServisKaydiIslemleri", name: "ServisId", newName: "Id2");
        }
    }
}
