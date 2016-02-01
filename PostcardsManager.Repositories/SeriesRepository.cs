using System;
using System.Data.Entity;
using System.Linq;
using PostcardsManager.Models;

namespace PostcardsManager.Repositories
{
    public class SeriesRepository
    {
        public IQueryable<Series> GetAll(out IDisposable context)
        {
            var db = new PostcardContext();
            context = db;

            return db.Series.Include(x => x.Publisher).Include(x => x.Postcards);
        }

        public Series GetById(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Series.Include(x => x.Publisher).Include(x => x.Postcards).First(x => x.Id == id);

                return model;
            }
        }

        public long Add(Series model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Series.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }

        public long Update(Series model)
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
                var model = context.Series.First(x => x.Id == id);
                context.Entry(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
