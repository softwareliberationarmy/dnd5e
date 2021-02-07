using System.Threading.Tasks;

namespace DnD_5e.Terminal.Common
{
    public interface ICommandProcessor
    {
        bool Matches(string input);
        Task Process(string input);
    }
}