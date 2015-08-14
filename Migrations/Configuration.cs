using System.Data.Entity.Migrations;
using PostcardsManager.DAL;
using PostcardsManager.Models;
using PostcardsManager.Properties;

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
            var storage = new Storage
            {
                PublicKey = Settings.Default.PublicKey,
                PrivateKey = Settings.Default.PrivateKey,
                StorageName = "Demo",
                Enabled = true
            };

            context.Storages.Add(storage);
            context.SaveChanges();
        }
    }
}