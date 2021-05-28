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
            var allUsers = _context.AppUsers.Include(e=>e.Interests);
            var currentUserInterests = allUsers.First();
            Match(currentUserInterests, allUsers.Skip(1), 5, MatchOptions.Both);
            return View();
        }

        // Matches current user with other users on interests, hobbies or both. Default: Interests.
        public IEnumerable<AppUser> Match(AppUser currentUser, IEnumerable<AppUser> allUsers, int returnCount, MatchOptions matchOptions = MatchOptions.Interests)
        {
            if(matchOptions == MatchOptions.Interests)
                return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count()).OrderByDescending(g => g.Key).SelectMany(u => u).Take(returnCount);

            if(matchOptions==MatchOptions.Hobbies)
                return allUsers.GroupBy(e => e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).SelectMany(u => u).Take(returnCount);

            return allUsers.GroupBy(e => e.Interests.Where(m => currentUser.Interests.Contains(m)).Count() + e.Hobbies.Where(m => currentUser.Hobbies.Contains(m)).Count()).OrderByDescending(g => g.Key).SelectMany(u => u).Take(returnCount);
        }

        public enum MatchOptions
        {
            Interests,
            Hobbies,
            Both
        }
    }

}
