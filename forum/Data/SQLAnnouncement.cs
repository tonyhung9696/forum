using forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class SQLAnnouncement
    {
        private readonly AppDbContext context;

        public SQLAnnouncement(AppDbContext context)
        {
            this.context = context;
        }

        public Announcement Add(Announcement item)
        {
            context.Announcement.Add(item);
            context.SaveChanges();
            return item;
        }

        public Announcement Delete(int Id)
        {
            Announcement item = context.Announcement.Find(Id);
            if (item != null)
            {
                context.Announcement.Remove(item);
                context.SaveChanges();
            }
            return item;
        }

        public IEnumerable<Announcement> GetAlltem()
        {
            return context.Announcement;
        }

        public Announcement GetEmployee(int Id)
        {
            return context.Announcement.Find(Id);
        }

        public Announcement Update(Announcement itemChanges)
        {
            var item = context.Announcement.Attach(itemChanges);
            item.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return itemChanges;
        }
    }
}
