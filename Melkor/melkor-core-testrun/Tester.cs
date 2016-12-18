using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace melkor_core_testrun
{
    public class Tester
    {
        public bool RunTest(string filePath)
        {
            Console.Out.WriteLine("Hello, I'm Melkor Test Runner");
            Console.Out.WriteLine(" Got assembly : " + filePath);
           AssemblyName an = AssemblyName.GetAssemblyName(filePath);
            Assembly asbly = Assembly.Load(an);

            if (asbly == null)
                Console.WriteLine(" I'm unable to load assembly... :(");
            else
                Console.WriteLine(asbly.FullName);

            Console.WriteLine("I found the folowing structure :");
            foreach (Type oType in asbly.GetTypes())
            {
                Console.WriteLine(" |-- "+oType.Name);
                foreach (var methods in oType.GetMethods())
                {
                   Console.WriteLine("    |-- " + methods.ToString());
                }
            }
            return true;
        }
       
    }
}
