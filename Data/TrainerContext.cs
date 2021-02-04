using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data
{
    public class TrainerContext: IdentityDbContext<Trainer>
    {
        public TrainerContext(DbContextOptions<TrainerContext> options): base(options) {   }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
