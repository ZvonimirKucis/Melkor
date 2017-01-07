using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public interface INotificationRepo
    {
        void Add(NotificationContext notification);
        bool Remove(Guid notificationId);
        void Edit(Guid notificationId, NotificationContext context);
        List<NotificationContext> GetContexts(int n);
    }
}
