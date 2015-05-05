namespace ConsoleX
{
    public class ConsoleCommandInstance
    {
        public ConsoleCommandInstance(IConsole console, IConsoleCommand command, string[] arguments)
        {
            Console = console;
            Command = command;
            Arguments = arguments;
        }

        public ConsoleCommandInstance(IConsole console, IConsoleCommand command)
            : this(console, command, new string[0])
        {
        }

        public IConsole Console { get; private set; }
        public IConsoleCommand Command { get; private set; }
        public string[] Arguments { get; private set; }
    }
}