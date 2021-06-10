using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Data
{
    public class Lidl
    {

        private const string mockUserDataFilePath = "./Data/MockData/MockUsers.json";
        private static ApplicationDbContext context;
        private static Random omegaRandom = new Random();
        public async static Task LidlSeed(ApplicationDbContext context)
        {
            
            var users = JsonConvert.DeserializeObject<IEnumerable<AppUser>>(File.ReadAllText(mockUserDataFilePath));
            List<Interest> interests = await context.Interests.ToListAsync();
            List<Hobby> hobbies = await context.Hobbies.ToListAsync();

            var img = context.UserImgs.Find(1);

            foreach (var user in users)
            {
                user.UserImg = img;
                user.UserName = user.Email;
                user.Interests = new List<Interest>();
                user.Hobbies = new List<Hobby>();
                var interestsCount = omegaRandom.Next(1, 10);
                var hobbiesCount = omegaRandom.Next(Math.Max(0, 3 - interestsCount), 10);
                for (int i = 0; i < interestsCount; i++)
                {
                    bool czech = true;
                    while (czech)
                    {
                        var interestIndex = omegaRandom.Next(interests.Count);
                        if (!user.Interests.Contains(interests[interestIndex]))
                        {
                            user.Interests.Add(interests[interestIndex]);
                            czech = false;
                        }
                    }
                }
                for (int i = 0; i < hobbiesCount; i++)
                {
                    bool czech = true;
                    while (czech)
                    {
                        var hobbyIndex = omegaRandom.Next(hobbies.Count);
                        if (!user.Hobbies.Contains(hobbies[hobbyIndex]))
                        {
                            user.Hobbies.Add(hobbies[hobbyIndex]);
                            czech = false;
                        }
                    }
                }
            }
            await context.AppUsers.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
