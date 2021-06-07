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
            var allUsers = _context.AppUsers.Include(e => e.Interests).Include(e => e.Hobbies).Include(e => e.UserImg);

            //TEST KOD
            var inte = _context.Interests.AsEnumerable();
            var hobb = _context.Hobbies.AsEnumerable();
            var result= GetSearchedUsers(inte.Take(1), hobb.Take(1), allUsers);
            //TEST KOD SLUT

            var currentUser = allUsers.Include(u => u.UserImg).First();

            var viewModel = new MatchViewModel(currentUser);

            // General Matches
            var generalMatches = Match(currentUser, allUsers.Skip(1), 10, MatchOptions.General);
            viewModel.GeneralMatch = new GeneralMatchViewModel(currentUser, generalMatches);

            // Hobby Matches
            var hobbyMatches = Match(currentUser, allUsers.Skip(1), 10, MatchOptions.Hobbies);
            viewModel.HobbyMatch = new HobbyMatchViewModel(currentUser, hobbyMatches);

            // Interest Matches
            var interestMatches = Match(currentUser, allUsers.Skip(1), 10, MatchOptions.Interests);
            viewModel.InterestMatch = new InterestMatchViewModel(currentUser, interestMatches);


            // Location Matches
            var locationMatches = Match(currentUser, allUsers.Skip(1), 20, MatchOptions.Location);
            viewModel.LocationMatch = new LocationMatchViewModel(currentUser, locationMatches);

            // Gör en sökning med IT och Cooking som hobby/interest parametrar
            var interests = _context.Interests.Where(i => i.Name == "IT").ToList();
            var hobbies = _context.Hobbies.Where(h => h.Name == "Cooking").ToList();
            var searchResults = GetSearchedUsers(allUsers, interests, hobbies);
            viewModel.SearchResult = new SearchResultViewModel { Users = searchResults };

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

        // TODO: Not working properly. Doesn't return the correct amount of users. Reason: IGrouping.
        // Matches current user with other users on interests, hobbies or both. Default: Interests.
        //public IEnumerable<IGrouping<int, AppUser>> Match(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Interests)
        //{
        //    if (matchOptions == MatchOptions.Interests)

        //        return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

        //    if (matchOptions == MatchOptions.Hobbies)
        //        return allUsers.GroupBy(e => e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

        //    return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count() + e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);
        //}

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
        public IEnumerable<AppUser> GetSearchedUsers(IEnumerable<Interest> interests, IEnumerable<Hobby> hobbies, IEnumerable<AppUser> users)
        {
            return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));
        }

        public IEnumerable<AppUser> GetSearchedUsers(AppUser currentUser, IEnumerable<AppUser> users, IEnumerable<Interest> interests , IEnumerable<Hobby> hobbies, int locationFilter, int minAge, int maxAge)
        {
            if (!interests.Contains(null) && hobbies.Contains(null))
                return users.Where(u => interests.All(i => u.Interests.Contains(i)));

            if (interests.Contains(null) && !hobbies.Contains(null))
                return users.Where(u => hobbies.All(h => u.Hobbies.Contains(h)));

            if (!interests.Contains(null) && !hobbies.Contains(null))
                return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));

            return users;
        }

        //public IEnumerable<AppUser> InterestHobbyFilter(IEnumerable<AppUser> users, IEnumerable<Interest> interests, IEnumerable<Hobby> hobbies)
        //{
        //    if (!interests.Contains(null) && hobbies.Contains(null))
        //        return users.Where(u => interests.All(i => u.Interests.Contains(i)));

        //    if (interests.Contains(null) && !hobbies.Contains(null))
        //        return users.Where(u => hobbies.All(h => u.Hobbies.Contains(h)));

        //    if (!interests.Contains(null) && !hobbies.Contains(null))
        //        return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));

        //    return users;
        //}

        //public IEnumerable<AppUser> LocationFilter(AppUser currentUser, IEnumerable<AppUser> users, int locationFilter)
        //{
        //    if (locationFilter == 1)
        //        return users.Where(u => u.Country == currentUser.Country);

        //    if (locationFilter == 2)
        //        return users.Where(u => u.City == currentUser.City);

        //    return users.ToList();
        //}

        //public IEnumerable<AppUser> AgeFilter(IEnumerable<AppUser> users, int minAge, int maxAge)
        //{
        //    return users.Where(u => u.Age >= minAge && u.Age <= maxAge).ToList();
        //}

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
