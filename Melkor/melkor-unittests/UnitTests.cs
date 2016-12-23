﻿using System;
using System.IO;
using melkor_core_testrun;
using Melkor_core_builder;
using Melkor_core_gitzipper;
using Microsoft.Build.Execution;
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
                "https://github.com/m3talen/RAUPJC-DZ2/",
                "https://github.com/fspigel/RAUPJC-DZ2/",
                "https://github.com/ib47885/DZ02",
                "https://github.com/tbozuric/RAUPJC-HW2",
                "https://github.com/nbukovac/RAUPJC_2.HW",
                "https://github.com/bernarda22/RAUPJC_DrugaDZ",
                "https://github.com/KatarinaBlazic/RAUPJC-DZ2",
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
        public void BuilderCs6()
        {
            float countPass = 0, countFail = 0;
            var target = @"C:\Melkor\";
            if (!Directory.Exists(target)) throw new DirectoryNotFoundException();
            var builder = new Builder(target);
            
            var res = builder.Build();
            foreach (var item in res)
            {
                if (item.Status) countPass++;
                else countFail++;
            }
            Console.WriteLine($"Pass : {countPass} \t Fail : {countFail}");
            Console.WriteLine($"Success rate : {(countPass/(countPass+countFail))*100} %");
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