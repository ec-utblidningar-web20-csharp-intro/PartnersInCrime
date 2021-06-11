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

        public AppUser thisUser { get; set; }
        public IFormFile image { get; set; }
        public UserImg ProfilePic { get; set; }

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

        public async Task<IActionResult> OnPostUploadAsync()
        {
            var file="";
            var userId = _userManager.GetUserId(User);
            var user = await _context.AppUsers.Where(u => u.Id == userId).Include(i => i.UserImg).FirstOrDefaultAsync();

            var defaultImgs = _context.UserImgs.Where(u => u.Id <= 5 && u.Id >= 1).ToList();

            if (defaultImgs.Any(i=>i.Name==image.FileName))
            {
                user.UserImg = defaultImgs.Where(i=>i.Name==image.FileName).FirstOrDefault();
                _context.SaveChanges();
                return Page();
            }

            file = BuildUrl(user.UserImg);
            System.IO.File.Delete(file);
            file = BuildUrl(image.FileName);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            var imgName = new UserImg { Name = image.FileName};
            _context.UserImgs.Add(imgName);
            thisUser = user;
            user.UserImg = imgName;
            _context.SaveChanges();
            return Page();
        }

        public async Task<IActionResult> OnPostDefaultAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.AppUsers.Where(u => u.Id == userId).Include(i => i.UserImg).FirstOrDefaultAsync();

            var imgId = Request.Form["profilepic"];

            var pp = _context.UserImgs.Where(u => u.Id == int.Parse(imgId)).FirstOrDefault();

            user.UserImg = pp;

            //var img = _context.UserImgs.Where(u => u.Id == defaultPicId).FirstOrDefault();

            thisUser = user;
            _context.SaveChanges();

            return Page();
        }
        public string BuildUrl(UserImg img)
        {
            var url = Path.Combine(_enviroment.ContentRootPath, "wwwroot/img", img.Name);
            return url;
        }
        public string BuildUrl(string fileName)
        {
            var url = Path.Combine(_enviroment.ContentRootPath, "wwwroot/img", fileName);
            return url;
        }
    }
}
