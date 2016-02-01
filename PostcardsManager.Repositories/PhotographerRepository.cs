using System;
using System.Data.Entity;
using System.Linq;
using PostcardsManager.Models;

namespace PostcardsManager.Repositories
{
    public class PhotographerRepository
    {
        public IQueryable<Photographer> GetAll(out IDisposable context)
        {
            var db = new PostcardContext();
            context = db;

            return db.Photographers;
        }

        public Photographer GetById(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Photographers.First(x => x.Id == id);

                return model;
            }
        }

        public long Add(Photographer model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Photographers.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }

        public long Update(Photographer model)
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
                var model = context.Photographers.First(x => x.Id == id);
                context.Entry(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
