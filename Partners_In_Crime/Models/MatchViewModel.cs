using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class MatchViewModel
    {
        public MatchViewModel(AppUser appUser, Hobby hobby = null, Interest interest = null)
        {
            AppUser = appUser;
            Hobby = hobby;
            Interest = interest;
        }
        public AppUser AppUser { get; set; }

        public Hobby Hobby { get; set; }
        public Interest Interest { get; set; }
        public GeneralMatchViewModel GeneralMatch { get; set; }
        public HobbyMatchViewModel HobbyMatch { get; set; }
        public InterestMatchViewModel InterestMatch { get; set; }
        public LocationMatchViewModel LocationMatch { get; set; }
        public SearchResultViewModel SearchResult { get; set; }
    }

    public class SearchResultViewModel
    {
        public IEnumerable<AppUser> Users { get; set; }
    }

    public class GeneralMatchViewModel
    {
        public GeneralMatchViewModel(AppUser appUser, IEnumerable<AppUser> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers;
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> MatchedUsers { get; set; }
    }

    public class HobbyMatchViewModel
    {
        public HobbyMatchViewModel(AppUser appUser, IEnumerable<AppUser> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers;
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> MatchedUsers { get; set; }
    }

    public class InterestMatchViewModel
    {
        public InterestMatchViewModel(AppUser appUser, IEnumerable<AppUser> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers;
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> MatchedUsers { get; set; }
    }

    public class LocationMatchViewModel
    {
        public LocationMatchViewModel(AppUser appUser, IEnumerable<AppUser> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers;
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<AppUser> MatchedUsers { get; set; }
    }
}
