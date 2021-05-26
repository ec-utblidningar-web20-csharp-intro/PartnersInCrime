using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Partners_In_Crime.Models;

namespace Partners_In_Crime.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Hobbies> Hobbies { get; set; }
        public DbSet<Interests> Interests { get; set; }
    }
}
