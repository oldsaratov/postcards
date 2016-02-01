using System.Data.Entity.Migrations;
using PostcardsManager.Repositories;

namespace PostcardsManager.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<PostcardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PostcardContext context)
        {
        }
    }
}