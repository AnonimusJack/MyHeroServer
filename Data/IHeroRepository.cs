using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data
{
    public interface IHeroRepository
    {
        public IEnumerable<Hero> Sync();
        public Hero GetHeroById(string id);
        public bool SaveChanges();
        public void InitialSeed();
    }
}
