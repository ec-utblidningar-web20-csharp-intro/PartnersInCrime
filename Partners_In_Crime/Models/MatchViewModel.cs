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
    }

    public class GeneralMatchViewModel
    {
        public GeneralMatchViewModel(AppUser appUser, IEnumerable<IGrouping<int, AppUser>> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers.Where(m => m.Key != 0);
            NotMatchedUsers = matchedUsers.Where(m => m.Key == 0);
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> MatchedUsers { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> NotMatchedUsers { get; set; }
    }

    public class HobbyMatchViewModel
    {
        public HobbyMatchViewModel(AppUser appUser, IEnumerable<IGrouping<int, AppUser>> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers.Where(m => m.Key != 0);
            NotMatchedUsers = matchedUsers.Where(m => m.Key == 0);
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> MatchedUsers { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> NotMatchedUsers { get; set; }
    }

    public class InterestMatchViewModel
    {
        public InterestMatchViewModel(AppUser appUser, IEnumerable<IGrouping<int, AppUser>> matchedUsers)
        {
            AppUser = appUser;
            MatchedUsers = matchedUsers.Where(m => m.Key != 0);
            NotMatchedUsers = matchedUsers.Where(m => m.Key == 0);
        }
        public AppUser AppUser { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> MatchedUsers { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> NotMatchedUsers { get; set; }
    }
}
