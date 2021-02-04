using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHeroServer.Data;
using MyHeroServer.Data.DTOs;
using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Controllers
{
    [Authorize(Roles = TrainerRoles.Trainer)]
    [ApiController]
    [Route("api/[controller]")]
    public class HeroesController: ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHeroRepository _repository;
        private readonly IMapper _mapper;

        public HeroesController(IHeroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPatch("train/{id}")]
        public ActionResult TrainHero(string id)
        {
            Hero hero = _repository.GetHeroById(id);
            if (hero == null)
            {
                log.Error($"Hero with id: {id} doesn't exist!");
                return NotFound();
            }
            if (hero.TrainingStamina == 0)
            {
                log.Error($"Hero with id: {hero.Id} is overtrained!");
                return StatusCode(409);
            }
            //random increase between 0%-10%
            float randomPercentage = (new Random().Next(11) / 100f);
            float powerIncrease = 1 + randomPercentage;
            hero.CurrentPower *= powerIncrease;
            hero.TrainingStamina--;
            GlobalValues.LastUpdated = DateTime.UtcNow;
            if (!_repository.SaveChanges())
            {
                log.Error("Changes were not saved!");
                return StatusCode(500);
            }
            log.Info($"{hero.Name} was trained by {powerIncrease * 100}% successfuly!");
            return Ok(new ServerResponse() { Status = "Success", Message = $"{hero.TrainingStamina}" }); 
        }

        [HttpGet("sync")]
        public ActionResult Sync()
        {
            List<Hero> heroes = _repository.Sync().ToList();
            return Ok(_mapper.Map<List<OutputHero>>(heroes));
        }

        [HttpGet("sync/{date}")]
        public ActionResult SyncRequired(string date)
        {
            DateTime clientLastUpdated = DateTime.Parse(date);
            if (clientLastUpdated >= GlobalValues.LastUpdated)
                return Ok(new ServerResponse() { Status = "Success", Message = "false" });
            else
                return Ok(new ServerResponse() { Status = "Success", Message = "true" });
        }
    }
}
