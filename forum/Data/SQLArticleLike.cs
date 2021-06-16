using forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class SQLArticleLike
    {
        private readonly AppDbContext context;

        public SQLArticleLike(AppDbContext context)
        {
            this.context = context;
        }

        public ArticleLike Add(ArticleLike item)
        {
            context.ArticleLike.Add(item);
            context.SaveChanges();
            return item;
        }

        public ArticleLike Delete(int Id)
        {
            ArticleLike item = context.ArticleLike.Find(Id);
            if (item != null)
            {
                context.ArticleLike.Remove(item);
                context.SaveChanges();
            }
            return item;
        }
        public void DeleteItems(IEnumerable<ArticleLike> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    context.ArticleLike.Remove(item);
                }
            }
            context.SaveChanges();
        }
        public IEnumerable<ArticleLike> GetAlltem()
        {
            return context.ArticleLike;
        }

        public ArticleLike GetDetail(int Id)
        {
            return context.ArticleLike.Find(Id);
        }

        public ArticleLike Update(ArticleLike itemChanges)
        {
            var item = context.ArticleLike.Attach(itemChanges);
            item.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return itemChanges;
        }
    }
}
