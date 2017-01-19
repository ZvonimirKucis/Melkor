using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Melkor_core_dbhandler;
using Melkor_core_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Cli.Utils.CommandParsing;

namespace Melkor_core_web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ITestRepo testRepo;
        private INotificationRepo notifyRepo;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, INotificationRepo notificationRepo, ITestRepo testRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.notifyRepo = notificationRepo;
            this.testRepo = testRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

            [AllowAnonymous]
        public async Task<IActionResult> Admini()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userManager.GetRolesAsync(user);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(NotificationContext news)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            NotificationContext data = new NotificationContext(news.Title, news.Message, user.UserName);
            notifyRepo.Add(data);
            return RedirectToAction("Index");
        }

        

        [AllowAnonymous]
        public async Task<IActionResult> Setup()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Administrator");
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
            }

            return RedirectToAction("Index");
        }

        public IActionResult TestResultsFailed()
        {
            return View(testRepo.GetAllTests(false));
        }

        public IActionResult TestResultsPassed()
        {
            return View(testRepo.GetAllTests(true));
        }
    }
}