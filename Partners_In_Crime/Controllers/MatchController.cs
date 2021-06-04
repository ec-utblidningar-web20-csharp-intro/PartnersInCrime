﻿using Microsoft.AspNetCore.Mvc;
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

            //TEST KOD
            var inte = _context.Interests.AsEnumerable();
            var hobb = _context.Hobbies.AsEnumerable();
            var result= GetSearchedUsers(inte.Take(1), hobb.Take(1), allUsers);
            //TEST KOD SLUT

            var currentUser = allUsers.First();

            var viewModel = new MatchViewModel(currentUser);

            //// General Matches
            //var generalMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Both);
            //viewModel.GeneralMatch = new GeneralMatchViewModel(currentUser, generalMatches);

            //// Hobby Matches
            //var hobbyMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Hobbies);
            //viewModel.HobbyMatch = new HobbyMatchViewModel(currentUser, hobbyMatches);

            //// Interest Matches
            //var interestMatches = Match(currentUser, allUsers.Skip(1), 5, MatchOptions.Interests);
            //viewModel.InterestMatch = new InterestMatchViewModel(currentUser, interestMatches);

            var bellaMatchUsers = BellaMatching(currentUser, allUsers.Skip(1), 10, MatchOptions.Both);
            viewModel.GeneralMatch = new GeneralMatchViewModel(currentUser, bellaMatchUsers);

            return View(viewModel);
        }

        // Matches current user with other users on interests, hobbies or both. Default: Interests.
        public IEnumerable<IGrouping<int, AppUser>> Match(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Interests)
        {
            if (matchOptions == MatchOptions.Interests)

                return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

            if (matchOptions == MatchOptions.Hobbies)
                return allUsers.GroupBy(e => e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);

            return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count() + e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).Take(returnCount);
        }

        public IEnumerable<AppUser> BellaMatching(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Interests)
        {
            if (matchOptions == MatchOptions.Interests)
            {
                var matchedUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i))).ToList();
                foreach (var user in matchedUsers)
                {
                    var allInterests = _context.Interests.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                    user.AmountOfMatchingParameters = allInterests.Count();
                }
                matchedUsers.OrderByDescending(m => m.AmountOfMatchingParameters).Take(returnCount);
                return matchedUsers;
            }
            if (matchOptions == MatchOptions.Hobbies)
            {
                var matchedUsers = allUsers.Where(u => currentUser.Hobbies.Any(i => u.Hobbies.Contains(i))).ToList();
                foreach (var user in matchedUsers)
                {
                    var allInterests = _context.Hobbies.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                    user.AmountOfMatchingParameters = allInterests.Count();
                }

                var result = matchedUsers.OrderBy(u => u.AmountOfMatchingParameters);

                return result;
            }

            // General match (hobbies and interests)
            var generalMatchUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i))).ToList();
            var additionalUsers = allUsers.Where(u => currentUser.Hobbies.Any(h => u.Hobbies.Contains(h))).ToList();

            var duplicates = new List<AppUser>();

            // Find duplicates in general/additional users
            foreach (var user in generalMatchUsers)
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

            generalMatchUsers.AddRange(additionalUsers);

            foreach (var user in generalMatchUsers)
            {
                var allInterests = _context.Interests.Where(i => i.AppUsers.Contains(user) && i.AppUsers.Contains(currentUser));
                var allHobbies = _context.Hobbies.Where(h => h.AppUsers.Contains(user) && h.AppUsers.Contains(currentUser));                
                user.AmountOfMatchingParameters = allInterests.Count() + allHobbies.Count();
            }
            
            return generalMatchUsers.OrderByDescending(u => u.AmountOfMatchingParameters).Take(returnCount);
        }

        public IEnumerable<AppUser> LidlMatchning(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Both)
        {
            if (matchOptions == MatchOptions.Interests)
            {

                var matchedUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i)) && u.City==currentUser.City).Take(returnCount);

                if(matchedUsers.Count() < returnCount)
                {
                    matchedUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i)) && u.Country == currentUser.Country).Take(returnCount - matchedUsers.Count());
                    if(matchedUsers.Count()< returnCount)
                    {
                        matchedUsers = allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i))).Take(returnCount - matchedUsers.Count());
                    }
                    return matchedUsers;
                }
                return matchedUsers;
            }

            if (matchOptions == MatchOptions.Hobbies)
            {
                var matchedUsers = allUsers.Where(u => currentUser.Hobbies.Any(i => u.Hobbies.Contains(i)) && u.City == currentUser.City).Take(returnCount);

                if (matchedUsers.Count() < returnCount)
                {
                    matchedUsers = allUsers.Where(u => currentUser.Hobbies.Any(i => u.Hobbies.Contains(i)) && u.Country == currentUser.Country).Take(returnCount - matchedUsers.Count());
                    if (matchedUsers.Count() < returnCount)
                    {
                        matchedUsers = allUsers.Where(u => currentUser.Hobbies.Any(i => u.Hobbies.Contains(i))).Take(returnCount - matchedUsers.Count());
                    }
                    return matchedUsers;
                }
                return matchedUsers;
            }
                return allUsers.Where(u => currentUser.Hobbies.Any(h => u.Hobbies.Contains(h))).Take(returnCount);

            return allUsers.Where(u => currentUser.Interests.Any(i => u.Interests.Contains(i)) || currentUser.Hobbies.Any(h => u.Hobbies.Contains(h))).Take(returnCount);
        }

       
        public enum MatchOptions
        {
            Interests,
            Hobbies,
            Both
        }

        public IEnumerable<AppUser> GetSearchedUsers(IEnumerable<Interest> interests, IEnumerable<Hobby> hobbies, IEnumerable<AppUser> users)
        {
            return users.Where(u => interests.All(i => u.Interests.Contains(i)) && hobbies.All(h => u.Hobbies.Contains(h)));
        }
    }

}
