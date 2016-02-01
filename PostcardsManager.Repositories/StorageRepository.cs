using System;
using System.Data.Entity;
using System.Linq;
using PostcardsManager.Models;

namespace PostcardsManager.Repositories
{
    public class StorageRepository
    {
        public IQueryable<Storage> GetAll(out IDisposable context)
        {
            var db = new PostcardContext();
            context = db;

            return db.Storages;
        }

        public Storage GetById(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Storages.First(x => x.Id == id);

                return model;
            }
        }

        public long Add(Storage model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Storages.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }

        public long Update(Storage model)
        {
            using (var context = new PostcardContext())
            {
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();

                return model.Id;
            }
        }

        public void Delete(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Storages.First(x => x.Id == id);
                context.Entry(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
