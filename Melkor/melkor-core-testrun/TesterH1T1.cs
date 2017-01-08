using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Melkor_core_dbhandler;

namespace melkor_core_testrun
{
    public class TesterH1T1
    {
        private readonly Type _listType;
        private string _path;
        private readonly Guid _userGuid;

        public TesterH1T1(string DLLPath, Guid userGuid)
        {
            this._userGuid = userGuid;
            DLLPath = DllHelper.FindDll(DLLPath, "IntegerList");
            _path = DLLPath;
            Console.WriteLine(DLLPath);

            using (Stream stream = File.OpenRead(DLLPath))
            {
                byte[] rawAssmebly = new byte[stream.Length];
                stream.Read(rawAssmebly, 0, (int)stream.Length);
                var asm = Assembly.Load(rawAssmebly);
                var listTypeName = asm.GetTypes().Where(a => a.ToString().ToLower().Contains(".integerlist")).Select(a => a.ToString()).FirstOrDefault();
                _listType = asm.GetType(listTypeName);
            }
        }

        public List<TestContext> RunTest()
        {
            var res = new List<TestContext>
            {
                new TestContext("ListContainsAddedElement", ListContainsAddedElement(), _userGuid),
                new TestContext("RemovingElementFromList", RemovingElementFromList(), _userGuid)
            };

            return res;
        }

        private bool ListContainsAddedElement()
        {
            object[] classConstructorArgs = { null };

            dynamic listInstance = Activator.CreateInstance(_listType, classConstructorArgs);
            listInstance.Add(10);
            return listInstance.Contains(10);
        }

        private bool RemovingElementFromList()
        {
            object[] classConstructorArgs = { null };

            dynamic listInstance = Activator.CreateInstance(_listType, classConstructorArgs);

            listInstance.Add(2);
            return listInstance.Remove(2);
        }
    }
}
