using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Partners_In_Crime.Data;
using Partners_In_Crime.Models;

namespace Partners_In_Crime.Areas.Identity.Pages.Account.Manage
{
    [BindProperties]
    public partial class ProfilePictureModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IHostEnvironment _enviroment;


        public ProfilePictureModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context,
            IHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _enviroment = environment;

        }

        public UserImg userPicture { get; set; }
        public AppUser thisUser { get; set; }
        public IFormFile image { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.AppUsers.Where(u => u.Id == userId).Include(i=>i.UserImg).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            thisUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var file="";
            var userId = _userManager.GetUserId(User);
            var user = await _context.AppUsers.Where(u => u.Id == userId).Include(i => i.UserImg).FirstOrDefaultAsync();
            if (user.UserImg != null && user.UserImg.Url != "/img/DefaultProfilePic.jpg")
            {
                file = Path.Combine(_enviroment.ContentRootPath, "wwwroot/img", user.UserImg.Url);
                System.IO.File.Delete(file);
            }
            file = Path.Combine(_enviroment.ContentRootPath, "wwwroot/img", image.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            var img = new UserImg { Url = image.FileName };
            _context.UserImgs.Add(img);
            
            
            thisUser = user;
            user.UserImg = img;
            _context.SaveChanges();
            return Page();
        }
    }
}
