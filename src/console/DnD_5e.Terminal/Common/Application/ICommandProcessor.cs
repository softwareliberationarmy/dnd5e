using System.Threading.Tasks;

namespace DnD_5e.Terminal.Common.Application
{
    public interface ICommandProcessor
    {
        bool Matches(string input);
        Task Process(string input);
    }
}