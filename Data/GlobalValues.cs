using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHeroServer.Data
{
    public static class GlobalValues
    {
        static GlobalValues()
        {
            LastUpdated = DateTime.UtcNow;
        }

        public static DateTime LastUpdated { get; set; }
    }
}
