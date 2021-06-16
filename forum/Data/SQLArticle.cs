using forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class SQLArticle
    {
        private readonly AppDbContext context;

        public SQLArticle(AppDbContext context)
        {
            this.context = context;
        }

        public Article Add(Article item)
        {
            context.Article.Add(item);
            context.SaveChanges();
            return item;
        }

        public Article Delete(int Id)
        {
            Article item = context.Article.Find(Id);
            if (item != null)
            {
                context.Article.Remove(item);
                context.SaveChanges();
            }
            return item;
        }

        public IEnumerable<Article> GetAlltem()
        {
            return context.Article;
        }

        public Article GetDetail(int Id)
        {
            return context.Article.Find(Id);
        }

        public Article Update(Article itemChanges)
        {
            var item = context.Article.Attach(itemChanges);
            item.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return itemChanges;
        }
    }
}
