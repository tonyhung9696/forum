using forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class SQLArticleReply
    {
        private readonly AppDbContext context;

        public SQLArticleReply(AppDbContext context)
        {
            this.context = context;
        }

        public ArticleReply Add(ArticleReply item)
        {
            context.ArticleReply.Add(item);
            context.SaveChanges();
            return item;
        }

        public ArticleReply Delete(int Id)
        {
            ArticleReply item = context.ArticleReply.Find(Id);
            if (item != null)
            {
                context.ArticleReply.Remove(item);
                context.SaveChanges();
            }
            return item;
        }
        public void DeleteItems(IEnumerable<ArticleReply> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    context.ArticleReply.Remove(item);
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<ArticleReply> GetAlltem()
        {
            return context.ArticleReply;
        }
        public IEnumerable<ArticleReply> GetArticleReplyList(string Id)
        {
            return context.ArticleReply.Where(e=>e.articleID==Id);
        }
        public ArticleReply GetDetail(int Id)
        {
            return context.ArticleReply.Find(Id);
        }

        public ArticleReply Update(ArticleReply itemChanges)
        {
            var item = context.ArticleReply.Attach(itemChanges);
            item.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return itemChanges;
        }
    }
}
