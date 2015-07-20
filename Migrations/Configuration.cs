using System.Data.Entity.Migrations;
using PostcardsManager.DAL;

namespace PostcardsManager.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SchoolContext context)
        {
        }
    }
}