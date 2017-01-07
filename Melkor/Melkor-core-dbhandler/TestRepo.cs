using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melkor_core_dbhandler
{
    public class TestRepo : ITestRepo
    {
        private readonly MelkorDb _context;

        public TestRepo(MelkorDb context)
        {
            _context = context;
        }
        public void Add(TestContext test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
        }
        
        public void Edit(Guid testId, TestContext test)
        {
            var temp = _context.Tests.Where(t => t.TestId.Equals(testId)).Select(t => t).First();
            if (temp == null)
            {
                _context.Tests.Add(test);
                _context.SaveChanges();
                return;
            }
            temp.Name = test.Name;
            temp.Result = test.Result;
            temp.RunDateTime = test.RunDateTime;
            _context.SaveChanges();
        }
    }
}
