using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;

namespace DnD_5e.Terminal.Roll
{
    public class RollCommandProcessor: ICommandProcessor
    {
        private readonly IDndApi _api;

        public RollCommandProcessor(IDndApi api)
        {
            _api = api;
        }

        public bool Matches(string input)
        {
            var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            return splits.Length > 1 && splits[0].ToLower().Trim() == "roll";
        }

        public async Task Process(string input)
        {
            var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            var roll = splits[1];

            var result = await _api.FreeRoll(roll);
            
            Console.WriteLine(result.Result);
        }
    }
}
