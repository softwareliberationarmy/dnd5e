using System;
using System.Collections.Generic;
using System.Text;

namespace DnD_5e.Domain.Roleplay
{
    public class Ability
    {
        public Ability(int score, bool proficientAtSaves)
        {
            Score = score;
            ProficientAtSaves = proficientAtSaves;
        }

        public int Score { get; }
        public bool ProficientAtSaves { get; }

    }
}
