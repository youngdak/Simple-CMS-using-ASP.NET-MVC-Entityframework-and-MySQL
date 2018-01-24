namespace MySql.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sermondateunique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Sermon", "Date", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sermon", new[] { "Date" });
        }
    }
}
