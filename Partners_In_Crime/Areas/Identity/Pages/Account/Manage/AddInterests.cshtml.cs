using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Partners_In_Crime.Data;
using Partners_In_Crime.Models;

namespace Partners_In_Crime.Areas.Identity.Pages.Account.Manage
{
    [BindProperties]
    public class AddInterestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddInterestsModel(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Hobby> Hobbies { get; set; }
        public List<Interest> Interests { get; set; }
        public List<Hobby> SelectedHobbies { get; set; }
        public List<Interest> SelectedInterests { get; set; }
        public void OnGet()
        {
            var interests = _context.Interests.ToList();
            Interests = interests;
            var hobbies = _context.Hobbies.ToList();
            Hobbies = hobbies;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.AppUsers.Where(u => u.Id == userId).Include(i => i.Hobbies).Include(i => i.Interests).FirstOrDefaultAsync();

            return null;
        }
    }
}
