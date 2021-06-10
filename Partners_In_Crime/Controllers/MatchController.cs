using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Partners_In_Crime.Data;
using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Controllers
{
    public class MatchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MatchController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {

            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }
            var userId = _userManager.GetUserId(User);

            var allUsers = _context.AppUsers.Include(e => e.Interests).Include(e => e.Hobbies).Include(e => e.UserImg).Where(u => u.Id != userId);
            
            var currentUser = _context.AppUsers.Include(u => u.Interests).Include(u => u.Hobbies).Where(u => u.Id == userId).FirstOrDefault();

            var viewModel = new MatchViewModel(currentUser);

            // General Matches
            var generalMatches = Match(currentUser, allUsers, 10, MatchOptions.General);
            viewModel.GeneralMatch = new GeneralMatchViewModel(currentUser, generalMatches);

            // Hobby Matches
            var hobbyMatches = Match(currentUser, allUsers, 10, MatchOptions.Hobbies);
            viewModel.HobbyMatch = new HobbyMatchViewModel(currentUser, hobbyMatches);

            // Interest Matches
            var interestMatches = Match(currentUser, allUsers, 10, MatchOptions.Interests);
            viewModel.InterestMatch = new InterestMatchViewModel(currentUser, interestMatches);

            // Location Matches
            var locationMatches = Match(currentUser, allUsers, 10, MatchOptions.Location);
            viewModel.LocationMatch = new LocationMatchViewModel(currentUser, locationMatches);

            //TEST KOD SÖKNING (alla som gillar Beer & Football, och är mellan 18 och 100 år) inget locationfilter på, den är 0
            var inte = _context.Interests.Where(i => i.Name == "Beer").AsEnumerable();
            var hobb = _context.Hobbies.Where(i => i.Name == "Football").AsEnumerable();
            var searchResult = GetSearchedUsers(currentUser, allUsers, inte, hobb, 0, 18, 100);
            //TEST KOD SLUT

            viewModel.SearchResult = new SearchResultViewModel { Users = searchResult };
            
            return View(viewModel);
        }

        public IEnumerable<AppUser> Match(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.General)
        {
            // Interest top matches
            if (matchOptions == MatchOptions.Interests)
            {
                var interestMatches = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i))).AsEnumerable();

                foreach (var user in interestMatches)
                {
                    var matchingInterests = _context.Interests.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                    user.MatchingInterests = matchingInterests.Count();
                }

                return interestMatches.OrderByDescending(m => m.MatchingInterests).Take(returnCount);
            }

            // Hobby top matches
            if (matchOptions == MatchOptions.Hobbies)
            {
                var hobbyMatches = allUsers.Where(u => currentUser.Hobbies.Any(i => u.Hobbies.Contains(i))).AsEnumerable();

                foreach (var user in hobbyMatches)
                {
                    var matchingHobbies = _context.Hobbies.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                    user.MatchingHobbies = matchingHobbies.Count();
                }

                return hobbyMatches.OrderByDescending(u => u.MatchingHobbies).Take(returnCount);
            }

            // Location top matches
            if (matchOptions == MatchOptions.Location)
            {
                var generalMatches = GeneralMatch(currentUser, allUsers, returnCount);
                
                foreach (var user in generalMatches)
                {
                    if (user.City == currentUser.City)
                    {
                        user.MatchingLocations ++;
                    }

                    if (user.Country == currentUser.Country) 
                    {
                        user.MatchingLocations ++;
                    }
                }
                return generalMatches.OrderByDescending(u => u.MatchingLocations).ThenByDescending(u => u.MatchingParameters);
            }

            // General Top Matches
            return GeneralMatch(currentUser, allUsers, returnCount);
        }

        public IEnumerable<AppUser> GeneralMatch(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount)
        {
            var matchedUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i))).ToList();
            var additionalUsers = allUsers.Where(u => currentUser.Hobbies.Any(h => u.Hobbies.Contains(h))).ToList();

            RemoveDuplicates(matchedUsers, additionalUsers);

            matchedUsers.AddRange(additionalUsers);

            foreach (var user in matchedUsers)
            {
                var allInterests = _context.Interests.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                var allHobbies = _context.Hobbies.Where(h => h.AppUsers.Contains(user) && h.AppUsers.Contains(currentUser));
                user.MatchingParameters = allInterests.Count() + allHobbies.Count();
            }

            return matchedUsers.OrderByDescending(u => u.MatchingParameters).Take(returnCount);
        }

        public void RemoveDuplicates(IEnumerable<AppUser> matchedUsers, List<AppUser> additionalUsers)
        {
            var duplicates = new List<AppUser>();

            // Find duplicates in general/additional users
            foreach (var user in matchedUsers)
            {
                foreach (var additionalUser in additionalUsers)
                {
                    if (user == additionalUser)
                    {
                        duplicates.Add(additionalUser);
                    }
                }
            }

            // Remove duplicates before adding to generalMatchUsers
            foreach (var duplicate in duplicates)
            {
                if (additionalUsers.Contains(duplicate))
                {
                    additionalUsers.Remove(duplicate);
                }
            }
        }

        public enum MatchOptions
        {
            Interests,
            Hobbies,
            Location,
            General
        }

        // For the search/filter thingy
        public IEnumerable<AppUser> GetSearchedUsers(AppUser currentUser, IEnumerable<AppUser> users, IEnumerable<Interest> interests , IEnumerable<Hobby> hobbies, int locationFilter, int minAge, int maxAge)
        {
            users = InterestHobbyFilter(users, interests, hobbies);
            users = AgeFilter(users, minAge, maxAge);
            users = LocationFilter(currentUser, users, locationFilter);

            return users;
        }

        public IEnumerable<AppUser> InterestHobbyFilter(IEnumerable<AppUser> users, IEnumerable<Interest> interests, IEnumerable<Hobby> hobbies)
        {
            if (!interests.Contains(null) && hobbies.Contains(null))
                return users.Where(u => interests.All(i => u.Interests.Contains(i)));
                
            if (interests.Contains(null) && !hobbies.Contains(null))
                return users = users.Where(u => hobbies.All(h => u.Hobbies.Contains(h)));

            if (!interests.Contains(null) && !hobbies.Contains(null))
                return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));

            return users;
        }

        public IEnumerable<AppUser> LocationFilter(AppUser currentUser, IEnumerable<AppUser> users, int locationFilter)
        {
            if (locationFilter == 2)
                return users.Where(u => u.Country == currentUser.Country);

            if (locationFilter == 3)
                return users.Where(u => u.City == currentUser.City);

            return users;
        }

        public IEnumerable<AppUser> AgeFilter(IEnumerable<AppUser> users, int minAge, int maxAge)
        {
            return users.Where(u => u.Age >= minAge && u.Age <= maxAge).ToList();
        }
    }
}
