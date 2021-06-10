using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Partners_In_Crime.Models;

namespace Partners_In_Crime.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Age")]
            public int Age { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }


        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var age = user.Age;
            var description = user.Description;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var country = user.Country;
            var city = user.City;

            Username = userName;

            Input = new InputModel
            {
                Age = age,
                Description = description,
                FirstName = firstName,
                LastName = lastName,
                Country = country,
                City = city
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var age = user.Age;
            var description = user.Description;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var country = user.Country;
            var city = user.City;

            if (Input.Age != age || Input.Description != description || Input.FirstName != firstName || Input.LastName != lastName || Input.Country != country || Input.City != city)
            {
                user.Age = Input.Age;
                user.Description = Input.Description;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Country = Input.Country;
                user.City = Input.City;

                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

       // public async Task<ActionResult> Update
    }
}
