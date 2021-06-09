using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class AppUser:IdentityUser
    {
        
        public int Age { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public int MatchingInterests { get; set; }
        public int MatchingHobbies { get; set; }
        public int MatchingParameters { get; set; }
        public int MatchingLocations { get; set; }

        public List<Interest> Interests { get; set; }
        public List<Hobby> Hobbies { get; set; }
        public UserImg UserImg { get; set; }
    }
}
