namespace DnD_5e.Terminal.Common.IO
{
    public interface IOutputWriter
    {
        void WriteLine(string line);
        string Prefix { get; }
    }
}