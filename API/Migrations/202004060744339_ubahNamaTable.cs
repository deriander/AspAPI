namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ubahNamaTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DepartmentModels", newName: "TB_M_Department");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TB_M_Department", newName: "DepartmentModels");
        }
    }
}
