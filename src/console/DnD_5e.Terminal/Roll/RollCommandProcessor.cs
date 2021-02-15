using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using DnD_5e.Terminal.Common.Application;
using DnD_5e.Terminal.Common.Interfaces;
using DnD_5e.Terminal.Common.IO;

namespace DnD_5e.Terminal.Roll
{
    public class RollCommandProcessor: ICommandProcessor
    {
        private readonly IDndApi _api;
        private readonly IOutputWriter _writer;

        public RollCommandProcessor(IDndApi api, IOutputWriter writer)
        {
            _api = api;
            _writer = writer;
        }

        public bool Matches(string input)
        {
            var pieces = Split(input);
            return pieces.Length > 1 && pieces[0].ToLower().Trim() == "roll";
        }

        public async Task Process(string input)
        {
            var pieces = Split(input);
            var roll = pieces[1];

            try
            {
                var result = await _api.FreeRoll(roll);
                _writer.WriteLine($"{result.Result}");
            }
            catch (ApiException ex)
            {
                _writer.WriteLine(ex.Message);
            }
        }

        protected string[] Split(string input)
        {
            var splits = input.Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
            return splits;
        }
    }
}
