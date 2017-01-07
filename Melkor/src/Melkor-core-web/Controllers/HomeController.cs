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

            List<NotificationContext> notifies = new List<NotificationContext>();
            notifies.Add(
                new NotificationContext("Title 1",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Admin"));
            notifies.Add(
                new NotificationContext("Title 2",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Admin1"));
            notifies.Add(
                new NotificationContext("Title 3",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Admin2"));
            return View(notifies);
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
            
            ViewData["Message"] = "Build at " + output;
            
            return View();
        }

        public async Task<ActionResult> BuildResult()
        {
            Builder builder = new Builder(_location);

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            string output = _location + @"\" + Guid.Parse(currentUser.Id).ToString() + @"\output";
            
            var resultBuildItems = builder.Build(output);

            TesterH2T1 tester = new TesterH2T1(output);
            foreach (var element in tester.RunTest())
            {
                resultBuildItems.Add(item: new BuildItem(element.Key, element.Value));
            }

            return PartialView("BuildResultView", resultBuildItems);
        }
        
        public IActionResult Error()
        {
            return View();
        }

    }
}
