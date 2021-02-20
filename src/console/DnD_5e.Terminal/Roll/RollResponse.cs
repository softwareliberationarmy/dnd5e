using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_5e.Terminal.Roll
{
    public class RollResponse
    {
        public int Result { get; set; }
        public int[] Rolls { get; set; }
        public string RequestedRoll { get; set; }
    }
}
