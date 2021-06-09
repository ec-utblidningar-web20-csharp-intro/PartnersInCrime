using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Partners_In_Crime.Data;
using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProfileController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index(int id)
        {
            var userId = _userManager.GetUserId(User);
            var currentUser = _context.AppUsers.Find(userId);

            var viewModel = new ProfileViewModel { AppUser = currentUser};
            return View(viewModel);
        }
    }
}
