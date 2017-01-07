using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class MelkorDb : DbContext
    {
        public IDbSet<NotificationContext> Notification { get; set; }
        public IDbSet<TestContext> Tests { get; set; }

        public MelkorDb(string connectionString) : base(connectionString)
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

            modelBuilder.Entity<TestContext>().HasKey(t => t.TestId);
            modelBuilder.Entity<TestContext>().Property(t => t.Name);
            modelBuilder.Entity<TestContext>().Property(t => t.RunDateTime);
            modelBuilder.Entity<TestContext>().Property(t => t.Result);
            modelBuilder.Entity<TestContext>().Property(t => t.UserId);
        }
    }
}
