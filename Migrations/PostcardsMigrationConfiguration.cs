using System.Data.Entity.Migrations;
using PostcardsManager.DAL;

namespace PostcardsManager.Migrations
{
    public sealed class PostcardsMigrationConfiguration : DbMigrationsConfiguration<PostcardContext>
    {
        public PostcardsMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PostcardContext context)
        {
        }
    }
}