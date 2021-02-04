using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHeroServer.Data;
using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyHeroServer.Services
{
    public class RefreshHeroStaminaService : IHostedService, IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public RefreshHeroStaminaService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("Initializing Hero Alarm Service");
            TimeSpan runAt6AM = TimeSpan.FromHours(30 - DateTime.Now.TimeOfDay.TotalHours);
            _timer = new Timer(WakeupRefreshedHeroes, null, runAt6AM, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void WakeupRefreshedHeroes(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var heroContext = scope.ServiceProvider.GetRequiredService<HeroContext>();
                var heroes = heroContext.Heroes.Where(hero => hero.CurrentPower < 100f).ToArray();
                foreach (Hero hero in heroes)
                {
                    hero.TrainingStamina = 5;
                }
                bool changesSaved = heroContext.SaveChanges() >= 0;
                if (changesSaved)
                    log.Info("All heroes have been woken up and are feeling refreshed!");
                else
                    log.Error("Something happened and the heroes didn't get enough sleep!");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info("Hero Alarm Service is shutting down!");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
