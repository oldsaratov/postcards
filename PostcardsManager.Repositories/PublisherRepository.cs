using System;
using System.Data.Entity;
using System.Linq;
using PostcardsManager.Models;

namespace PostcardsManager.Repositories
{
    public class PublisherRepository
    {
        public IQueryable<Publisher> GetAll(out IDisposable context)
        {
            var db = new PostcardContext();
            context = db;

            return db.Publishers.Include(x => x.Series);
        }

        public Publisher GetById(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Publishers.Include(x => x.Series).First(x => x.Id == id);

                return model;
            }
        }

        public long Add(Publisher model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Publishers.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }

        public long Update(Publisher model)
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
                var model = context.Publishers.First(x => x.Id == id);
                context.Entry(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
