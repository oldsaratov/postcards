using System.Data.Entity;
using PostcardsManager.Models;
using PostcardsManager.Properties;

namespace PostcardsManager.DAL
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<PostcardContext>
    {
        protected override void Seed(PostcardContext context)
        {
            var storage = new Storage
            {
                PublicKey = Settings.Default.PublicKey,
                PrivateKey = Settings.Default.PrivateKey,
                StorageLimit = Settings.Default.StorageLimit,
                StorageName = "Demo"
            };

            context.Storages.Add(storage);
            context.SaveChanges();
        }
    }
}