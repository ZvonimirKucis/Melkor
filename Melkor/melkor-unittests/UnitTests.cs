using System;
using System.IO;
using melkor_core_testrun;
using Melkor_core_builder;
using Melkor_core_gitzipper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace melkor_unittests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void GitDownloadTest()
        {
            var downloadLocation = @"C:\Melkor\";
            string[] urls =
            {
                "https://github.com/ZvonimirKucis/2-domaca-zadaca",
                "https://github.com/fspigel/RAUPJC-DZ2/",
                "https://github.com/ib47885/DZ02",
                "https://github.com/Jebansi33/DrugaDZ",
                "https://github.com/nikolazinjo/RAUPJC-2DZ",
                "https://github.com/tbozuric/RAUPJC-HW2",
                "https://github.com/nbukovac/RAUPJC_2.HW",
                "https://github.com/bernarda22/RAUPJC_DrugaDZ",
                "https://github.com/KatarinaBlazic/RAUPJC-DZ2",
                "https://github.com/dafta/RAUPJC-2DZ",
                "https://github.com/donikv/dz2"
            };

            GitZipper.CleanUp(downloadLocation);
            foreach (var url in urls)
            {
                var zip = new GitZipper(url, Guid.NewGuid().ToString());

                zip.GitDownload(downloadLocation);
                zip.GitUnzip();
            }
        }


        [TestMethod]
        public void BuilderTestBuildv2()
        {
            var res = false;
            var target = @"C:\Melkor\";
            if (!Directory.Exists(target)) throw new DirectoryNotFoundException();
            var builder = new Builder(target);
            var strin = builder.FindProjectFile(target);
            foreach (var dir in strin)
            {
                res = builder.Build3(dir);
                Console.WriteLine("Building " + res + " -> " + dir);
            }
            GitZipper.CleanUp(target);
            Assert.AreEqual(true, res);
        }


        [TestMethod]
        public void TestRun()
        {
            var target = @"C:\Melkor\";

            var tester = new Tester();
            var res = tester.RunTest(target + "zad1.dll");

            Assert.AreEqual(true, res);
        }
    }
}