using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class TestContext
    {
        public string Name { get; set; }
        public bool Result { get; set; }
        public DateTime RunDateTime { get; set; }
        public Guid TestId { get; set; }
        public Guid UserId { get; set; }

        public TestContext(string name, bool result, Guid userId)
        {
            UserId = userId;
            Name = name;
            Result = result;
            RunDateTime = DateTime.Now;
            TestId = Guid.NewGuid();
        }

    }
}
