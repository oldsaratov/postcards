namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Postcard", "Title", c => c.String(maxLength: 150));
            AlterColumn("dbo.Postcard", "ImageLink", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Postcard", "ImageLink", c => c.String());
            AlterColumn("dbo.Postcard", "Title", c => c.String());
        }
    }
}
