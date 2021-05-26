using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (!context.AppUsers.Any())
            {
                Seed(context);
            }
        }

        public static void Seed(ApplicationDbContext context)
        {
            var interests = new List<Interests>
            {
                new Interests {Name = "Reading"}, new Interests {Name = "Movies"}, new Interests {Name = "Psychology"}, new Interests {Name = "Music"},
                new Interests {Name = "History"}, new Interests {Name = "Photography"}, new Interests {Name = "Geography"}, new Interests {Name = "Beer"},
                new Interests {Name = "Nature"}, new Interests {Name = "Global warming"}, new Interests {Name = "Pop culture"}, new Interests {Name = "Deep conversations"},
                new Interests {Name = "Animals"}, new Interests {Name = "IT"}, new Interests {Name = "Art"}, new Interests {Name = "Antique collecting"},
                new Interests {Name = "Social causes"}, new Interests {Name = "Languages"}, new Interests {Name = "Stock Trading"}, new Interests {Name = "Nutrition"}
            };

            foreach (var interest in interests)
            {
                context.Interests.Add(interest);
            }

            context.SaveChanges();

            var hobbies = new List<Hobbies>
            {
                new Hobbies {ID = 1, Name = "Football"}, new Hobbies {ID = 2, Name = "Tennis"}, new Hobbies {ID = 3, Name = "Gaming"}, new Hobbies {ID = 4, Name = "Cooking"},
                new Hobbies {ID = 5, Name = "Yoga"}, new Hobbies {ID = 6, Name = "Design"}, new Hobbies {ID = 7, Name = "Volonteering"}, new Hobbies {ID = 8, Name = "Travelling"},
                new Hobbies {ID = 9, Name = "Vlogging"}, new Hobbies {ID = 10, Name = "Excercising"}, new Hobbies {ID = 11, Name = "Piano"}, new Hobbies {ID = 12, Name = "Writing"},
                new Hobbies {ID = 13, Name = "Guitar"}, new Hobbies {ID = 14, Name = "Singing"}, new Hobbies {ID = 15, Name = "Baking"}, new Hobbies {ID = 16, Name = "Theater"},
                new Hobbies {ID = 17, Name = "Camping"}, new Hobbies {ID = 18, Name = "Hiking"}, new Hobbies {ID = 19, Name = "Skating"}, new Hobbies {ID = 20, Name = "Partying"}
            };

            foreach (var hobby in hobbies)
            {
                context.Hobbies.Add(hobby);
            }

            context.SaveChanges();

            Random rnd = new Random();

            var testUsers = new List<AppUser>()
            {
                new AppUser {ID = 1, Age = 23, Description = "Hej! Ge mig en vän :(", FirstName = "Fake", LastName = "Fakesson", Email = "fakeuser@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 2, Age = 50, Description = "Tjena! Letar nya vänner.", FirstName = "Alice", LastName = "Aagesdatter", Email = "aaagerdatter@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 3, Age = 34, Description = "Bli vän med mig, jag är bäst!", FirstName = "Noah", LastName = "Baark", Email = "noahb@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 4, Age = 18, Description = "Please be my friend..", FirstName = "Maja", LastName = "Cajander", Email = "majacaj@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 5, Age = 19, Description = "Ser fram emot nya vänner!", FirstName = "William", LastName = "D'Aubert", Email = "wdaubert@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 6, Age = 27, Description = "Skriv till mig people!", FirstName = "Elsa", LastName = "Ebbesen", Email = "eebbesen@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 7, Age = 32, Description = "Jag är sjukt trevlig.", FirstName = "Hugo", LastName = "Fabricius", Email = "hfabricius@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 8, Age = 10, Description = "Letar efter lika tråkiga vänner som jag.", FirstName = "Astrid", LastName = "Gemmeltoft", Email = "agemmel@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 9, Age = 45, Description = "Like me for who I am.", FirstName = "Lucas", LastName = "Hasselrot", Email = "lhasselrot@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }},

                new AppUser {ID = 10, Age = 62, Description = "Letar efter en skön typ.", FirstName = "Wilma", LastName = "Ipsen", Email = "wipsen@gmail.com",
                            interests = new List<Interests>() {interests[rnd.Next(21)], interests[rnd.Next(21)] }, hobbies = new List<Hobbies>() {hobbies[rnd.Next(21)], hobbies[rnd.Next(21)] }}
            };

            foreach (var testUser in testUsers)
            {
                context.AppUsers.Add(testUser);
            }

            context.SaveChanges();
        }
    }
}
