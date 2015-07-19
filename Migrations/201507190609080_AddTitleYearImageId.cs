namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitleYearImageId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcard", "Title", c => c.String());
            AddColumn("dbo.Postcard", "Year", c => c.Int());
            AddColumn("dbo.Postcard", "ImageId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Postcard", "ImageId");
            DropColumn("dbo.Postcard", "Year");
            DropColumn("dbo.Postcard", "Title");
        }
    }
}
