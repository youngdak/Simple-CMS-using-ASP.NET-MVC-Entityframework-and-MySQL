namespace MySql.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class corpermemberusernameunique : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CorpMember", "Username", c => c.String(maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.CorpMember", "Username", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CorpMember", new[] { "Username" });
            DropColumn("dbo.CorpMember", "Username");
        }
    }
}
