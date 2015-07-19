namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImageLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcard", "ImageLink", c => c.String());
            DropColumn("dbo.Postcard", "ImageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Postcard", "ImageId", c => c.Guid());
            DropColumn("dbo.Postcard", "ImageLink");
        }
    }
}
