using System;
using System.Collections.Generic;
using System.Text;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        public int Strength { get; set; }

        public string GetAbilityRoll(string ability)
        {
            var modifier = (Strength - 10) / 2;
            var roll = "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
            return roll;
        }
    }
}
