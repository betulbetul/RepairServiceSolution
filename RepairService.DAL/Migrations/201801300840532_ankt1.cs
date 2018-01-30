namespace RepairService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankt1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Anketler", "MusteriUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Anketler", new[] { "MusteriUserID" });
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
            
            DropColumn("dbo.Anketler", "MusteriUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anketler", "MusteriUserID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AnketMusteriler", "Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnketMusteriler", "Id1", "dbo.Anketler");
            DropIndex("dbo.AnketMusteriler", new[] { "Id2" });
            DropIndex("dbo.AnketMusteriler", new[] { "Id1" });
            DropTable("dbo.AnketMusteriler");
            CreateIndex("dbo.Anketler", "MusteriUserID");
            AddForeignKey("dbo.Anketler", "MusteriUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
