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
    public class TesterH1T2
    {
        private readonly Type _listType;
        private string _path;
        private readonly Guid _userGuid;

        public TesterH1T2(string DLLPath, Guid userGuid)
        {
            this._userGuid = userGuid;
            _path = DLLPath;

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
               new TestContext("ListContainsAddedElement",_path, ListContainsAddedElement(), _userGuid),
              // new TestContext("RemovingElementFromList",_path, RemovingElementFromList(), _userGuid)
            };

            return res;
        }

        private bool ListContainsAddedElement()
        {
            object[] classConstructorArgs = { 4 };
            if(_listType.IsGenericType)
            return true;
            dynamic listInstance = Activator.CreateInstance(_listType, classConstructorArgs);
            listInstance.Add(19);
            return listInstance.Contains(19);
        }

        private bool RemovingElementFromList()
        {
            object[] classConstructorArgs = { 4 };

            dynamic listInstance = Activator.CreateInstance(_listType, classConstructorArgs);

            listInstance.Add(2);
            return listInstance.Remove(2);
        }
    }
}
