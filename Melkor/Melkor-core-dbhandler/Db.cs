using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class Db : DbContext
    {
        public IDbSet<NotificationContext> Notification { get; set; }

        public Db(string connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationContext>().HasKey(n => n.NotificationId);
            modelBuilder.Entity<NotificationContext>().Property(n => n.Title);
            modelBuilder.Entity<NotificationContext>().Property(n => n.Message);
            modelBuilder.Entity<NotificationContext>().Property(n => n.DateCreated);
            modelBuilder.Entity<NotificationContext>().Property(n => n.PublishedBy);
        }
    }
}
