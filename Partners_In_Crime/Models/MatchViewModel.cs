using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class MatchViewModel
    {
        public MatchViewModel(AppUser appUser, IEnumerable<IGrouping<int, AppUser>> matchedUsers, Hobby hobby = null, Interest interest = null )
        {
            AppUser = appUser;
            Hobby = hobby;
            Interest = interest;
            MatchedUsers = matchedUsers.Where(m=>m.Key != 0);
            NotMatchedUsers = matchedUsers.Where(m => m.Key == 0);
        }
        public AppUser AppUser { get; set; }
        
        public Hobby Hobby { get; set; }
        public Interest Interest { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> MatchedUsers { get; set; }
        public IEnumerable<IGrouping<int, AppUser>> NotMatchedUsers { get; set; }

    }
}
