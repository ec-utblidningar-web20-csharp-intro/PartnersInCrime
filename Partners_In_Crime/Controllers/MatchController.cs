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
        public MatchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var allUsers = _context.AppUsers.Include(e => e.Interests).Include(e => e.Hobbies);
            var currentUser = allUsers.Include(u => u.UserImg).First();

            var viewModel = new MatchViewModel(currentUser);

            // General Matches
            var generalMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Both);
            viewModel.GeneralMatch = new GeneralMatchViewModel(currentUser, generalMatches);

            // Hobby Matches
            var hobbyMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Hobbies);
            viewModel.HobbyMatch = new HobbyMatchViewModel(currentUser, hobbyMatches);

            // Interest Matches
            var interestMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Interests);
            viewModel.InterestMatch = new InterestMatchViewModel(currentUser, interestMatches);

            // Gör en sökning med IT och Cooking som hobby/interest parametrar
            var interests = _context.Interests.Where(i => i.Name == "IT").ToList();
            var hobbies = _context.Hobbies.Where(h => h.Name == "Cooking").ToList();
            var searchResults = GetSearchedUsers(allUsers, interests, hobbies);
            viewModel.SearchResult = new SearchResultViewModel { Users = searchResults };

            return View(viewModel);
        }

        // Matches current user with other users on interests, hobbies or both. Default: Interests.
        public IEnumerable<IGrouping<int, AppUser>> Match(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Interests)
        {
            if(matchOptions == MatchOptions.Interests)
                return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

            if(matchOptions==MatchOptions.Hobbies)
                return allUsers.GroupBy(e => e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

            return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count() + e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);
        }

        public enum MatchOptions
        {
            Interests,
            Hobbies,
            Both
        }

        public IEnumerable<AppUser> GetSearchedUsers(IEnumerable<AppUser> users, IEnumerable<Interest> interests , IEnumerable<Hobby> hobbies)
        {
            if (!interests.Contains(null) && hobbies.Contains(null))
                return users.Where(u => interests.All(i => u.Interests.Contains(i)));

            if (interests.Contains(null) && !hobbies.Contains(null))
                return users.Where(u => hobbies.All(h => u.Hobbies.Contains(h)));

            if (!interests.Contains(null) && !hobbies.Contains(null)) 
                return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));

            return new List<AppUser>();
        }

        // Alternativ GetSearchedUsers metod
        //public List<AppUser> FindUsers(List<Hobby> hobbies = null, List<Interest> interests = null)
        //{
        //    var result = new List<AppUser>();

        //    var users = _context.AppUsers.Include(a => a.Hobbies).Include(a => a.Interests).ToList();

        //    var noMatchHobbies = new List<Hobby>();
        //    var matchedHobbies = new List<Hobby>();

        //    var noMatchInterests = new List<Interest>();
        //    var matchedInterests = new List<Interest>();

        //    foreach (var user in users)
        //    {
        //        if (hobbies != null)
        //        {
        //            noMatchHobbies = user.Hobbies.Except(hobbies).ToList();
        //            matchedHobbies = user.Hobbies.Except(noMatchHobbies).ToList();

        //            if (interests != null)
        //            {
        //                noMatchInterests = user.Interests.Except(interests).ToList();
        //                matchedInterests = user.Interests.Except(noMatchInterests).ToList();

        //                if (matchedHobbies.Count == hobbies.Count && matchedInterests.Count == interests.Count)
        //                {
        //                    result.Add(user);
        //                }
        //            }
        //        }
        //        if (interests != null)
        //        {
        //            noMatchInterests = user.Interests.Except(interests).ToList();
        //            matchedInterests = user.Interests.Except(noMatchInterests).ToList();

        //            if (matchedInterests.Count == interests.Count)
        //            {
        //                result.Add(user);
        //            }
        //        }
        //        if (matchedInterests.Count == interests.Count)
        //        {
        //            result.Add(user);
        //        }
        //    }

        //    return result;
        //}
    }
}
