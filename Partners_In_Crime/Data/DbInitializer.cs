using Microsoft.AspNetCore.Identity;
using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (!context.AppUsers.Any())
            {
                Seed(context, userManager).Wait();
                Lidl.LidlSeed(context).Wait();
            }
        }

        public static async Task Seed(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            var interests = new List<Interest>
            {
                new Interest {Name = "Reading"}, new Interest {Name = "Movies"}, new Interest {Name = "Psychology"}, new Interest {Name = "Music"},
                new Interest {Name = "History"}, new Interest {Name = "Photography"}, new Interest {Name = "Geography"}, new Interest {Name = "Beer"},
                new Interest {Name = "Nature"}, new Interest {Name = "Global warming"}, new Interest {Name = "Pop culture"}, new Interest {Name = "Deep conversations"},
                new Interest {Name = "Animals"}, new Interest {Name = "IT"}, new Interest {Name = "Art"}, new Interest {Name = "Antique collecting"},
                new Interest {Name = "Social causes"}, new Interest {Name = "Languages"}, new Interest {Name = "Stock Trading"}, new Interest {Name = "Nutrition"}
            };

            foreach (var interest in interests)
            {
                context.Interests.Add(interest);
            }

            context.SaveChanges();

            var hobbies = new List<Hobby>
            {
                new Hobby {Name = "Football"}, new Hobby {Name = "Tennis"}, new Hobby {Name = "Gaming"}, new Hobby {Name = "Cooking"},
                new Hobby {Name = "Yoga"}, new Hobby {Name = "Design"}, new Hobby {Name = "Volonteering"}, new Hobby {Name = "Travelling"},
                new Hobby {Name = "Vlogging"}, new Hobby {Name = "Excercising"}, new Hobby {Name = "Piano"}, new Hobby {Name = "Writing"},
                new Hobby {Name = "Guitar"}, new Hobby {Name = "Singing"}, new Hobby {Name = "Baking"}, new Hobby {Name = "Theater"},
                new Hobby {Name = "Camping"}, new Hobby {Name = "Hiking"}, new Hobby {Name = "Skating"}, new Hobby {Name = "Partying"}
            };

            foreach (var hobby in hobbies)
            {
                context.Hobbies.Add(hobby);
            }

            var img = new UserImg { Url = "/DefaultProfilePic.jpg" };
            context.UserImgs.Add(img);
            context.SaveChanges();

            var testUser = new AppUser
            {
                FirstName = "Test",
                LastName = "Testsson",
                UserName = "test@test.com",
                Email = "test@test.com",
                City = "Jinghai",
                Country = "China",
                Age = 23,
                Description = "HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej HEJ hej ",
                UserImg = img,
                Hobbies = new List<Hobby> { new Hobby { Name = hobbies[1].Name }, new Hobby { Name = hobbies[2].Name }, new Hobby { Name = hobbies[3].Name } },
                Interests = new List<Interest> { new Interest { Name = interests[1].Name }, new Interest { Name = interests[2].Name }, new Interest { Name = interests[3].Name } }
            };

            await userManager.CreateAsync(testUser, "Test1234%");

            context.SaveChanges();
        }

    }
}
