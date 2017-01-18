using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melkor_core_dbhandler;
using System.IO;
using System.Reflection;

namespace melkor_core_testrun
{
    public class TestPicker
    {
        private string _homeworkPath;
        private Guid _userId;

        public TestPicker(string homeworkPath, Guid userID)
        {
            _homeworkPath = homeworkPath;
            _userId = userID;
        }

        public List<TestContext> Test()
        {
            var files = Directory.GetFiles(_homeworkPath);
            List<TestContext> results = new List<TestContext>();
            foreach (var file in files)
            {
                if (file.EndsWith(".dll"))
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        byte[] rawAssmebly = new byte[stream.Length];
                        stream.Read(rawAssmebly, 0, (int) stream.Length);
                        var assembly = Assembly.Load(rawAssmebly);
                        foreach (var type in assembly.GetTypes())
                            if (type.ToString().ToLower().Contains(".todorepository"))
                            {
                                TesterH2T1 testH2T1 = new TesterH2T1(file,_userId);
                                results.AddRange(testH2T1.RunTest());
                            }
                            else if (type.ToString().ToLower().Contains(".integerlist"))
                            {
                                TesterH1T1 testH1T1 = new TesterH1T1(file,_userId);
                                results.AddRange(testH1T1.RunTest());
                                
                            }

                    }

                }
            }
            return results;
        }
    }
}
