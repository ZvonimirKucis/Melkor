using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> AdminI()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var user = await _userManager.FindByIdAsync(currentUser.Id);
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Setup() { 
        
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var user = await _userManager.FindByIdAsync(currentUser.Id);

           
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Administrator");
                await _roleManager.CreateAsync(adminRole);
            }

            if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(user, adminRole.Name);
                
            }

            return Ok();
        }

    }
}