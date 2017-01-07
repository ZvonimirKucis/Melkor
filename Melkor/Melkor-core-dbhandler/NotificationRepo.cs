using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly MelkorDb _context;

        public NotificationRepo(MelkorDb context)
        {
            _context = context;
        }

        public void Add(NotificationContext notification)
        {
            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public bool Remove(Guid notificationId)
        {
            var temp = _context.Notification.Where(n => n.NotificationId.Equals(notificationId)).Select(n => n).First();
            if (temp == null) return false;
            _context.Notification.Remove(temp);
            _context.SaveChanges();
            return true;
        }

        public void Edit(Guid notificationId, NotificationContext context)
        {
            var temp = _context.Notification.Where(n => n.NotificationId.Equals(notificationId)).Select(n => n).First();
            if (temp == null)
            {
                _context.Notification.Add(context);
                _context.SaveChanges();
                return;
            }
            temp.Title = context.Title;
            temp.Message = context.Message;
            temp.DateCreated = context.DateCreated;
            temp.PublishedBy = context.PublishedBy;

            _context.SaveChanges();
        }

        public List<NotificationContext> GetContexts(int n)
        {
            if (n > _context.Notification.ToList().Count) n = _context.Notification.ToList().Count;
            List<NotificationContext> notifications = new List<NotificationContext>(n);
            var temp = _context.Notification.OrderByDescending(x => x.DateCreated).ToList();
            for (int i = 0; i < n; i++)
                notifications.Add(temp.ElementAt(i));
            return notifications;
        }

    }
}
