using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyHeroServer.Data.Models.Hero;

namespace MyHeroServer.Data.DTOs
{
    public class OutputHero
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AbilityType Type { get; set; }
        public float CurrentPower { get; set; }
        public int TrainingStamina { get; set; }
    }
}
