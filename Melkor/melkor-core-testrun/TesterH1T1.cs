using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace melkor_core_testrun
{
    public class TesterH1T1
    {
        private readonly Type _listType;
        private Dictionary<string, bool> results;
        private string path;

        public TesterH1T1(string DLLPath)
        {
            DLLPath = DllHelper.FindDll(DLLPath, "IntegerList");
            path = DLLPath;
            Console.WriteLine(DLLPath);
            results = new Dictionary<string, bool>();
            using (Stream stream = File.OpenRead(DLLPath))
            {
                byte[] rawAssmebly = new byte[stream.Length];
                stream.Read(rawAssmebly, 0, (int)stream.Length);
                var asm = Assembly.Load(rawAssmebly);
                var listTypeName = asm.GetTypes().Where(a => a.ToString().ToLower().Contains(".integerlist")).Select(a => a.ToString()).FirstOrDefault();
                _listType = asm.GetType(listTypeName);
            }
        }

        public Dictionary<string, bool> RunTest()
        {
            results.Add("ListContainsAddedElement", ListContainsAddedElement());
            results.Add("RemovingElementFromList", RemovingElementFromList());
            return results;
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
