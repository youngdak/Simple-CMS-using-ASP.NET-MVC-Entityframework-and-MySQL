namespace MySql.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class searchtags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Batch", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.ServiceYear", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Articles", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.CorpMember", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Event", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.News", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.OperationToRoles", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Portfolio", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Project", "SearchTag", c => c.String(unicode: false));
            AddColumn("dbo.Sermon", "SearchTag", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sermon", "SearchTag");
            DropColumn("dbo.Project", "SearchTag");
            DropColumn("dbo.Portfolio", "SearchTag");
            DropColumn("dbo.OperationToRoles", "SearchTag");
            DropColumn("dbo.News", "SearchTag");
            DropColumn("dbo.Event", "SearchTag");
            DropColumn("dbo.CorpMember", "SearchTag");
            DropColumn("dbo.Articles", "SearchTag");
            DropColumn("dbo.ServiceYear", "SearchTag");
            DropColumn("dbo.Batch", "SearchTag");
            DropColumn("dbo.Album", "SearchTag");
        }
    }
}
