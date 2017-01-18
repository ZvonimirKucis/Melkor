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

        public List<TestContext> GetTest(Guid userId)
        {
            var temp = _context.Tests.Where(t => t.UserId.Equals(userId)).ToList();
            if(temp == null) throw new ArgumentNullException();
            return temp;
        }

        public List<TestContext> GetTest(Guid userId, bool passed)
        {
            var temp = _context.Tests.Where(t => t.UserId.Equals(userId)).Where(t => t.Result == passed).ToList();
            if (temp == null) throw new ArgumentNullException();
            return temp;
        }

        public List<TestContext> GetAllTests(bool passed)
        {
            var temp = _context.Tests.Where(t => t.Result == passed).ToList();
            if (temp == null) throw new ArgumentNullException();
            return temp;
        }
    }
}
