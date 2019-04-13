namespace CuStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Code", c => c.String(nullable: false, maxLength: 200));
            Sql("UPDATE dbo.Products SET Code = REPLACE(Name, ' ', '') WHERE Code = ''");

            CreateIndex("dbo.Products", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "Code" });
            DropColumn("dbo.Products", "Code");
        }
    }
}
