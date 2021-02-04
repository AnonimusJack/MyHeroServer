using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data
{
    public class HeroSqlRepository : IHeroRepository
    {
        private readonly HeroContext _context;
        public HeroSqlRepository(HeroContext context)
        {
            _context = context;
        }

        public Hero GetHeroById(string id)
        {
            return _context.Heroes.FirstOrDefault(hero => hero.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Hero> Sync()
        {
            return _context.Heroes.ToList();
        }

        public void InitialSeed()
        {
            _context.Heroes.AddRange(new List<Hero>() { 
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Midoriya Izuku", Ability = "One For All", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Green, Black, Red", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Attacker },
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Bakugō Katsuki", Ability = "Explosion", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Black, Red, Green", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Attacker },
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Uraraka Ochako", Ability = "Zero Gravity", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Pink, White, Black", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Support },
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Kirishima Eijirō", Ability = "Hardening", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Red, Black, Brown", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Defender },
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Yaoyorozu Momo", Ability = "Creation", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Red, White, Yellow", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Support },
                new Hero() { Id = Guid.NewGuid().ToString(), Name = "Todoroki Shōto", Ability = "Half-Cold Half-Hot", CurrentPower = 1f, StartingPower = 1f, SuitColors = "Navy, White", TrainingStamina = 5, TrainingStartDate = DateTime.Now, Type = Hero.AbilityType.Attacker }
            });
            _context.SaveChanges();
        }
    }
}
