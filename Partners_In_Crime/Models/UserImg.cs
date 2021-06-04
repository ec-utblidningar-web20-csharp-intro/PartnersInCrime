using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Models
{
    public class UserImg
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
