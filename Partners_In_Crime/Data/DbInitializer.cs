using Partners_In_Crime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Partners_In_Crime.Data
{
    public static class DbInitializer
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

            context.SaveChanges();

            Random rnd = new Random();

            var testUsers = new List<AppUser>()
            {
                new AppUser {Age = 23, Description = "Hej! Ge mig en vän :(", FirstName = "Fake", LastName = "Fakesson", Email = "fakeuser@gmail.com", Country = "Sweden", City = "Gothenburg",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 50, Description = "Tjena! Letar nya vänner.", FirstName = "Alice", LastName = "Aagesdatter", Email = "aaagerdatter@gmail.com", Country = "Sweden", City = "Stockholm",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 34, Description = "Bli vän med mig, jag är bäst!", FirstName = "Noah", LastName = "Baark", Email = "noahb@gmail.com", Country = "Sweden", City = "Mora",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 18, Description = "Please be my friend..", FirstName = "Maja", LastName = "Cajander", Email = "majacaj@gmail.com", Country = "Sweden", City = "Trelleborg",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 19, Description = "Ser fram emot nya vänner!", FirstName = "William", LastName = "D'Aubert", Email = "wdaubert@gmail.com", Country = "Sweden", City = "Arvika",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 27, Description = "Skriv till mig people!", FirstName = "Elsa", LastName = "Ebbesen", Email = "eebbesen@gmail.com", Country = "Netherlands", City = "Amsterdam",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 32, Description = "Jag är sjukt trevlig.", FirstName = "Hugo", LastName = "Fabricius", Email = "hfabricius@gmail.com", Country = "New Zeeland", City = "Queenstown",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 10, Description = "Letar efter lika tråkiga vänner som jag.", FirstName = "Astrid", LastName = "Gemmeltoft", Email = "agemmel@gmail.com", Country = "United Kingdom", City = "Liverpool",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},
                 
                new AppUser {Age = 45, Description = "Like me for who I am.", FirstName = "Lucas", LastName = "Hasselrot", Email = "lhasselrot@gmail.com", Country = "Sweden", City = "Malmö",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 62, Description = "Letar efter en skön typ.", FirstName = "Wilma", LastName = "Ipsen", Email = "wipsen@gmail.com", Country = "Spain", City = "Madrid",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 23, Description = "Hej! Ge mig en vän :(", FirstName = "Lars", LastName = "Larsson", Email = "larsa@gmail.com", Country = "Spain", City = "Madrid",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 50, Description = "Tjena! Letar nya vänner.", FirstName = "Gösta", LastName = "Bark", Email = "gbark@gmail.com", Country = "Sweden", City = "Stockholm",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 34, Description = "Bli vän med mig, jag är bäst!", FirstName = "Leo", LastName = "Igelström", Email = "Ligel@gmail.com", Country = "New Zeeland", City = "Auckland",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 18, Description = "Please be my friend..", FirstName = "Maja", LastName = "Lindström", Email = "majjal@gmail.com", Country = "Netherlands", City = "Amsterdam",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 19, Description = "Ser fram emot nya vänner!", FirstName = "William", LastName = "Karlsson", Email = "Wkarlsson@gmail.com", Country = "Sweden", City = "Jönköping",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 27, Description = "Skriv till mig people!", FirstName = "Caroline", LastName = "Ebbesen", Email = "ceebbesen@gmail.com", Country = "Sweden", City = "Örebro",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 32, Description = "Jag är sjukt trevlig.", FirstName = "Leonardo", LastName = "Fabricius", Email = "lfabricius@gmail.com", Country = "Italy", City = "Rome",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 10, Description = "Letar efter lika tråkiga vänner som jag.", FirstName = "Astrid", LastName = "Gemmeltoft", Email = "agemmel@gmail.com", Country = "United Kingdom", City = "Liverpool",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 45, Description = "Like me for who I am.", FirstName = "Nora", LastName = "Hasselrot", Email = "nhasselrot@gmail.com", Country = "Sweden", City = "Trelleborg",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }},

                new AppUser {Age = 62, Description = "Letar efter en skön typ.", FirstName = "Klas", LastName = "Ipsen", Email = "kipsen@gmail.com", Country = "Italy", City = "Venice",
                            Interests = new List<Interest>() {interests[rnd.Next(interests.Count)], interests[rnd.Next(interests.Count)] }, Hobbies = new List<Hobby>() {hobbies[rnd.Next(interests.Count)], hobbies[rnd.Next(interests.Count)] }}
            };

            foreach (var testUser in testUsers)
            {
                context.AppUsers.Add(testUser);
            }

            context.SaveChanges();
        }
    }
}
