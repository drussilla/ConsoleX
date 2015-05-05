using System;

namespace ConsoleX
{
    public class ConsoleCommand : IConsoleCommand
    {
        public ConsoleCommand(string name, Action<string[]> action, Func<bool> isFinished, Func<string, string[]> autoComplete)
        {
            Name = name;
            Action = action;
            IsFinished = isFinished;
            AutoComplete = autoComplete;
        }

        public ConsoleCommand(string name, Action<string[]> action, Func<bool> isFinished) 
            : this(name, action, isFinished, s => new string[0])
        {
        }

        public ConsoleCommand(string name, Action<string[]> action, Func<string, string[]> autoComplete)
            : this(name, action, () => true, autoComplete)
        {
        }

        public ConsoleCommand(string name, Action<string[]> action)
            : this(name, action, () => true, s => new string[0])
        {
        }

        public string Name { get; private set; }

        public Action<string[]> Action { get; private set; }

        public Func<string, string[]> AutoComplete { get; private set; }

        public Func<bool> IsFinished { get; private set; }
    }
}