using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class NotificationContext
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public string PublishedBy { get; set; }
        public Guid NotificationId { get; set; }

        public NotificationContext(string title, string message, string publishedBy)
        {
            NotificationId = Guid.NewGuid();
            Title = title;
            Message = message;
            PublishedBy = publishedBy;
            DateCreated = DateTime.Now;
        }

        public NotificationContext()
        {
            
        }

    }
}
