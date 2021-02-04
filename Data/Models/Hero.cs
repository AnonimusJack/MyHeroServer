using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data.Models
{
    public class Hero
    {
        private float _currentPower;
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Ability { get; set; }
        [Required]
        public AbilityType Type { get; set; }
        [Required]
        public DateTime TrainingStartDate { get; set; }
        public string SuitColors { get; set; }
        [Required]
        public float StartingPower { get; set; }
        public float CurrentPower
        {
            get { return _currentPower; }
            set
            {
                _currentPower = value;
                if (_currentPower >= 100f)
                {
                    _currentPower = 100f;
                    TrainingStamina = 0;
                }
            }
        }
        public int TrainingStamina { get; set; }


        public enum AbilityType
        {
            Attacker,
            Defender,
            Support
        }
    }
}
