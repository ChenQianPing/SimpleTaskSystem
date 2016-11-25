namespace SimpleTaskSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.People", newName: "StsPeople");
            RenameTable(name: "dbo.Tasks", newName: "StsTasks");
            AlterColumn("dbo.StsTasks", "State", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StsTasks", "State", c => c.Int(nullable: false));
            RenameTable(name: "dbo.StsTasks", newName: "Tasks");
            RenameTable(name: "dbo.StsPeople", newName: "People");
        }
    }
}
