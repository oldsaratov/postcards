using System;
using System.Data.Entity;
using System.Linq;
using PostcardsManager.Models;
using System.Collections.Generic;

namespace PostcardsManager.Repositories
{
    public class PostcardsRepository
    {
        public IQueryable<Postcard> GetAll(out IDisposable context)
        {
            var db = new PostcardContext();
            context = db;

            return db.Postcards.Include(x => x.Photographer).Include(x => x.Series).Include(x => x.ImageFront).Include(x => x.ImageBack);
        }

        public Postcard GetById(long id)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Postcards.Include(x => x.Photographer).Include(x => x.Series).Include(x => x.ImageFront).Include(x => x.ImageBack).First(x => x.Id == id);

                return model;
            }
        }

        public Postcard GetAnyPostcardByPublisher(long publisherId)
        {
            using (var context = new PostcardContext())
            {
                var publisher = context.Publishers.Include(x => x.Series).First(x => x.Id == publisherId);
                var series = context.Series.FirstOrDefault(x => x.Postcards.Any());
                if (series != null)
                    return context.Postcards.FirstOrDefault(x => x.SeriesId == series.Id);
                return null;
            }
        }

        public IEnumerable<Postcard> GetBySeriesId(long seriesId)
        {
            using (var context = new PostcardContext())
            {
                var model = context.Postcards.Include(x => x.ImageFront).Include(x => x.ImageBack).Where(x => x.SeriesId == seriesId).ToList();

                return model;
            }
        }

        public long Add(Postcard model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Postcards.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }


        public long AddImage(Image model)
        {
            using (var context = new PostcardContext())
            {
                var result = context.Images.Add(model);
                context.Entry(result).State = EntityState.Added;
                context.SaveChanges();

                return result.Id;
            }
        }

        public long Update(Postcard model)
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
                var model = context.Postcards.First(x => x.Id == id);
                context.Entry(model).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
