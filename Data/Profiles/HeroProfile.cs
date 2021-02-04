using AutoMapper;
using MyHeroServer.Data.DTOs;
using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data.Profiles
{
    public class HeroProfile: Profile
    {
        public HeroProfile()
        {
            CreateMap<Hero, OutputHero>();
        }
    }
}
