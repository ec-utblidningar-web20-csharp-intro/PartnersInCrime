using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Partners_In_Crime.Models;

namespace Partners_In_Crime.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<UserImg> UserImgs { get; set; }
    }
}
