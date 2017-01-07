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

        public HomeController(IHostingEnvironment environment, UserManager<ApplicationUser> userManager, IConfigurationRoot configuration)
        {
            this._environment = environment;
            this._userManager = userManager;
            this._location = configuration.GetSection("Environment")["Storage"];
        }

        public IActionResult Index()
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
