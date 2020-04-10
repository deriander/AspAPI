namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDeptAndDiv : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_M_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdateDate = c.DateTimeOffset(precision: 7),
                        DeleteDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TB_M_Division",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdateDate = c.DateTimeOffset(precision: 7),
                        DeleteDate = c.DateTimeOffset(precision: 7),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TB_M_Department", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_M_Division", "DepartmentID", "dbo.TB_M_Department");
            DropIndex("dbo.TB_M_Division", new[] { "DepartmentID" });
            DropTable("dbo.TB_M_Division");
            DropTable("dbo.TB_M_Department");
        }
    }
}
