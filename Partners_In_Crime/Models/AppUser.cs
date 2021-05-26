using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class AppUser
    {
        public int ID { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<Interests> interests { get; set; }
        public List<Hobbies> hobbies { get; set; }
    }
}
