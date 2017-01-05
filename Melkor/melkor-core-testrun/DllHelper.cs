using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace melkor_core_testrun
{
    public class DllHelper
    {
        public static string FindDll(string dir)
        {
            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
            Evidence adevidence = AppDomain.CurrentDomain.Evidence;
            AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);

            Type typeP = typeof(Proxy);
            var value = (Proxy) domain.CreateInstanceAndUnwrap(
                typeP.Assembly.FullName,
                typeP.FullName);

            var files = Directory.GetFiles(dir);
                
            foreach (var file in files)
            {
                if (file.EndsWith(".dll"))
                {
                    var assembly = value.GetAssembly(file);
                    foreach (var type in assembly.GetTypes())
                        if (type.ToString().ToLower().Contains(".todorepository"))
                        {
                            AppDomain.Unload(domain);
                            return file;
                        }
                }
            }
            return null;
        }
    }

    class Proxy : MarshalByRefObject
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
