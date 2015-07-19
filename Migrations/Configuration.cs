using System.Data.Entity.Migrations;
using ContosoUniversity.DAL;

namespace ContosoUniversity.Migrations
{
    internal sealed class Configuration  : DbMigrationsConfiguration<SchoolContext>
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
