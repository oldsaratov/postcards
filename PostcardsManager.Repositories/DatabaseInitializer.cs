using PostcardsManager.Models;
using PostcardsManager.Repositories.Properties;
using System.Data.Entity;

namespace PostcardsManager.Repositories
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<PostcardContext>
    {
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