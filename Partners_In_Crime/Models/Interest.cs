using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class Interest
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<AppUser> AppUsers { get; set; }

    }
}
