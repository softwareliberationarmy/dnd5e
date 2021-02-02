using System;
using System.Collections.Generic;
using System.Text;

namespace DnD_5e.Domain.Common
{
    public static class ExperiencePointExtensions
    {
        public static int ToCharacterLevel(this int experiencePoints)
        {
            var levels = new[]
            {
                0, 300, 900, 2700, 6500, 14000, 23000, 34000, 48000,
                64000, 85000, 100000, 120000, 140000, 165000, 195000,
                225000, 265000, 305000, 355000
            };
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] > experiencePoints)
                {
                    return i;
                }
            }

            return 20;
        }
    }
}
