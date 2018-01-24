namespace MySql.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumPhoto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        YearId = c.Int(nullable: false),
                        BatchId = c.Int(nullable: false),
                        ContentType = c.String(unicode: false),
                        Photo = c.Binary(),
                        Description = c.String(unicode: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Album", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceYear", t => t.YearId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.YearId)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250, storeType: "nvarchar"),
                        SubZone = c.String(unicode: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Batch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearId = c.Int(nullable: false),
                        BatchName = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceYear", t => t.YearId, cascadeDelete: true)
                .Index(t => t.YearId);
            
            CreateTable(
                "dbo.ServiceYear",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.String(nullable: false, maxLength: 9, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Year, unique: true);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(unicode: false),
                        Article = c.String(unicode: false),
                        Display = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                        Username = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CorpMember", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.CorpMember",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearId = c.Int(nullable: false),
                        BatchId = c.Int(nullable: false),
                        Positions = c.String(unicode: false),
                        PositionIds = c.String(unicode: false),
                        FullName = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        OtherName = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        Gender = c.String(unicode: false),
                        Day = c.Int(nullable: false),
                        Month = c.String(unicode: false),
                        Year = c.Int(nullable: false),
                        StateOfOrigin = c.String(unicode: false),
                        Lga = c.String(unicode: false),
                        MaritalStatus = c.String(nullable: false, unicode: false),
                        PersonalInfo = c.String(unicode: false),
                        AcademicInfo = c.String(unicode: false),
                        NccfInfo = c.String(unicode: false),
                        PpaTown = c.String(unicode: false),
                        IsGeneral = c.Boolean(nullable: false),
                        IsLeader = c.Boolean(nullable: false),
                        MiniImage = c.Binary(),
                        MiniContentType = c.String(unicode: false),
                        MajorImage = c.Binary(),
                        MajorContentType = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceYear", t => t.YearId, cascadeDelete: true)
                .Index(t => t.YearId)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.Carousel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Img = c.Binary(),
                        ContentType = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(unicode: false),
                        StartDay = c.String(unicode: false),
                        StartMonth = c.String(unicode: false),
                        StartYear = c.String(unicode: false),
                        start = c.DateTime(nullable: false, precision: 0),
                        end = c.String(unicode: false),
                        StartTime = c.String(unicode: false),
                        EndTime = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Venue = c.String(unicode: false),
                        VenueAddress = c.String(unicode: false),
                        url = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Give",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Donate = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewHere",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewToNccf = c.String(unicode: false),
                        WhatToExpect = c.String(unicode: false),
                        FamilySong = c.String(unicode: false),
                        ServiceTimesandLocations = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(unicode: false),
                        ContentType = c.String(unicode: false),
                        Photo = c.Binary(),
                        TheNews = c.String(unicode: false),
                        Display = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                        Username = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CorpMember", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.OperationToRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceId = c.Int(nullable: false),
                        ResourceName = c.String(unicode: false),
                        RoleName = c.String(unicode: false),
                        Operations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Portfolio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.String(maxLength: 250, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Position, unique: true);
            
            CreateTable(
                "dbo.ProjectPicture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        ContentType = c.String(unicode: false),
                        Photo = c.Binary(),
                        Description = c.String(unicode: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250, storeType: "nvarchar"),
                        SubZone = c.String(unicode: false),
                        Milestone = c.String(unicode: false),
                        DateCreated = c.DateTime(nullable: false, precision: 0),
                        DateModified = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RoleId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sermon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        Scriptures = c.String(unicode: false),
                        Message = c.String(unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WhoWeAre",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Whoweare = c.String(unicode: false),
                        OurHistory = c.String(unicode: false),
                        OurBeliefs = c.String(unicode: false),
                        OurMission = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectPicture", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.News", "AuthorId", "dbo.CorpMember");
            DropForeignKey("dbo.Articles", "AuthorId", "dbo.CorpMember");
            DropForeignKey("dbo.CorpMember", "YearId", "dbo.ServiceYear");
            DropForeignKey("dbo.CorpMember", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.AlbumPhoto", "YearId", "dbo.ServiceYear");
            DropForeignKey("dbo.AlbumPhoto", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Batch", "YearId", "dbo.ServiceYear");
            DropForeignKey("dbo.AlbumPhoto", "AlbumId", "dbo.Album");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Project", new[] { "Name" });
            DropIndex("dbo.ProjectPicture", new[] { "ProjectId" });
            DropIndex("dbo.Portfolio", new[] { "Position" });
            DropIndex("dbo.News", new[] { "AuthorId" });
            DropIndex("dbo.CorpMember", new[] { "BatchId" });
            DropIndex("dbo.CorpMember", new[] { "YearId" });
            DropIndex("dbo.Articles", new[] { "AuthorId" });
            DropIndex("dbo.ServiceYear", new[] { "Year" });
            DropIndex("dbo.Batch", new[] { "YearId" });
            DropIndex("dbo.Album", new[] { "Name" });
            DropIndex("dbo.AlbumPhoto", new[] { "BatchId" });
            DropIndex("dbo.AlbumPhoto", new[] { "YearId" });
            DropIndex("dbo.AlbumPhoto", new[] { "AlbumId" });
            DropTable("dbo.WhoWeAre");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Sermon");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Project");
            DropTable("dbo.ProjectPicture");
            DropTable("dbo.Portfolio");
            DropTable("dbo.OperationToRoles");
            DropTable("dbo.News");
            DropTable("dbo.NewHere");
            DropTable("dbo.Give");
            DropTable("dbo.Event");
            DropTable("dbo.Carousel");
            DropTable("dbo.CorpMember");
            DropTable("dbo.Articles");
            DropTable("dbo.ServiceYear");
            DropTable("dbo.Batch");
            DropTable("dbo.Album");
            DropTable("dbo.AlbumPhoto");
        }
    }
}
