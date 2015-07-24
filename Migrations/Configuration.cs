using System.Data.Entity.Migrations;
using PostcardsManager.DAL;

namespace PostcardsManager.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<PostcardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PostcardContext context)
        {
        }
    }
}