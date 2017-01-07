using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace melkor_core_testrun
{
    /// <summary>
    ///     <p>Autor : Kucis</p>
    /// </summary>
    public class DllHelper
    {
        public static string FindDll(string dir, string className)
        {
            var files = Directory.GetFiles(dir);
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
                            if (type.ToString().ToLower().Contains("."+className.ToLower()))
                            {
                                return file;
                            }

                    }
                }
            }

            return null;
        }

    }
}



