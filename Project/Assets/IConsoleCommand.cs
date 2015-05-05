using System;

namespace ConsoleX
{
    public interface IConsoleCommand
    {
        string Name { get; }
        Action<string[]> Action { get; }
        Func<string, string[]> AutoComplete { get; }
        Func<bool> IsFinished { get; } 
    }
}