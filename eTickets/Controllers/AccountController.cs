using eTickets.Data;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            var usersWithRoles = new List<UserVM>();
            var travelManagers = await _context.AgencyManagers.Include(n => n.TravelAgency).ToListAsync();
            foreach (var user in users)
            {
                var userVm = new UserVM()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    UserName = user.UserName,
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.FirstOrDefault() == UserRoles.Manager)
                {
                    userVm.Role = UserRoles.Manager;
                    userVm.TravelAgency = travelManagers.Where(n => n.UserId == user.Id).FirstOrDefault().TravelAgency.Name;
                    //userVm.TravelAgencyId = travelManagers.Where(n => n.UserId == user.Id).FirstOrDefault().TravelAgency.Id;
                }
                else if (userRoles.FirstOrDefault() == UserRoles.Admin)
                {
                    userVm.Role = UserRoles.Admin;
                }
                else
                {
                    userVm.Role = UserRoles.User;
                }

                usersWithRoles.Add(userVm);
            }

            return View(usersWithRoles);
        }

        public async Task<IActionResult> AddManager(string id)
        {
            var users = await _context.Users.ToListAsync();
            var user = users.FirstOrDefault(n => n.Id == id);
            ViewBag.TravelAgencies = new SelectList(_context.TravelAgencies, "Id", "Name");

            return View(new UserVM()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddManager(UserVM userVm)
        {
            var dBUser = _context.Users.FirstOrDefault(n => n.Id == userVm.Id);
            await _userManager.AddToRoleAsync(dBUser, UserRoles.Manager);
            await _userManager.RemoveFromRoleAsync(dBUser, UserRoles.User);

            _context.AgencyManagers.Add(new AgencyManager()
            {
                TravelAgencyId = userVm.TravelAgencyId,
                UserId = userVm.Id,
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> RemoveManager(string id)
        {
            var dBUser = _context.Users.FirstOrDefault(n => n.Id == id);
            if (dBUser.Id != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                await _userManager.AddToRoleAsync(dBUser, UserRoles.User);
                await _userManager.RemoveFromRoleAsync(dBUser, UserRoles.Manager);

                var manager = _context.AgencyManagers.FirstOrDefault(x => x.UserId == id);
                if (manager != null)
                {
                    _context.AgencyManagers.Remove(manager);
                }

                await _context.SaveChangesAsync();    
            }
            return RedirectToAction("Users");
        }


        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Tours");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }


        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (!newUserResponse.Succeeded)
            {
                TempData["Error"] = "Password is not strong";
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Tours");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

    }
}
