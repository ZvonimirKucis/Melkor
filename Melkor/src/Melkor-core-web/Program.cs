using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Melkor_core_dbhandler;
using Microsoft.AspNetCore.Hosting;

namespace Melkor_core_web
{
    public class Program
    {
        public static void Main(string[] args)
        {;

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            
            host.Run();
        }
    }
}
