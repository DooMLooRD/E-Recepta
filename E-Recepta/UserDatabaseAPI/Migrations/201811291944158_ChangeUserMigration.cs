namespace UserDatabaseAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "LastName", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "Pesel", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Pesel");
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "Name");
        }
    }
}
