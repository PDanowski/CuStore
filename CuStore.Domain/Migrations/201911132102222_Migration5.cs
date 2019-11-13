namespace CuStore.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "Code" });
            AddColumn("dbo.AspNetUsers", "CrmGuid", c => c.Guid());
            AlterColumn("dbo.Products", "Code", c => c.String(nullable: false, maxLength: 200, unicode: false));
            CreateIndex("dbo.Products", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "Code" });
            AlterColumn("dbo.Products", "Code", c => c.String());
            DropColumn("dbo.AspNetUsers", "CrmGuid");
            CreateIndex("dbo.Products", "Code", unique: true);
        }
    }
}
