using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using melkor_core_testrun;
using Melkor_core_builder;
using Melkor_core_dbhandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Melkor_core_gitzipper;
using Melkor_core_web.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Melkor_core_web.Controllers
{

    public class HomeController : Controller
    {
        private readonly string _location;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        private ITestRepo testRepo;
        private INotificationRepo notifyRepo;

        public HomeController(IHostingEnvironment environment, UserManager<ApplicationUser> userManager, IConfigurationRoot configuration,INotificationRepo notificationRepo, ITestRepo testRepo)
        {
            this._environment = environment;
            this._userManager = userManager;
            this._location = configuration.GetSection("Environment")["Storage"];
            this.testRepo = testRepo;
            this.notifyRepo = notificationRepo;
        }

        public IActionResult Index()
        {
            var notifies = notifyRepo.GetContexts(5);
            return View(notifies);
        }

        public IActionResult AddFakeNews()
        {
            notifyRepo.Add(new NotificationContext("Demo title", "Demo tekst demo tekst demo tekst demo tekst demo tekst", "Admin"));
            return View("Index");
        }


        [Authorize]
        public IActionResult Tester()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GitDownload(GitLinkModel link)
        {
            if (!link.URL.Contains("github.com")) return View("Index");

            string downloadLocation = _location;
            
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            GitZipper.CleanUp(downloadLocation);
            
            GitZipper zip = new GitZipper(link.URL, Guid.Parse(currentUser.Id).ToString());
                
            zip.GitDownload(downloadLocation);
            zip.GitUnzip();

            ViewData["Message"] = link.URL;

            return RedirectToAction("Build");
        }

        [Authorize]
        public async Task<IActionResult> Build()
        {
            string target = _location;
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string output = _location + @"\" + Guid.Parse(currentUser.Id).ToString() + @"\output";
            
          //  ViewData["Message"] = "Build at " + output;
            
            return View();
        }

        public async Task<ActionResult> BuildResult()
        {
            Builder builder = new Builder(_location);

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string output = _location + @"\" + Guid.Parse(currentUser.Id).ToString() + @"\output";
            
            var resultBuildItems = builder.Build(output);
            
            TestPicker tester = new TestPicker(output, Guid.Parse(currentUser.Id));
            List<TestContext> results = tester.Test();
            if (results.Count != 0)
            {
                foreach (var element in tester.Test())
                {
                    var buildItem = resultBuildItems.FirstOrDefault(s => s.Dir.Equals(element.Dir));

                    buildItem?.Tests.Add(element);
                }
            }
            
            return PartialView("BuildResultView", resultBuildItems);
        }
        
        public IActionResult Error()
        {
            return View();
        }

    }
}
