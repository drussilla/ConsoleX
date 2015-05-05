using System;

namespace ConsoleX.Exceptions
{
    public class ConsoleCommandNotRegistered : Exception
    {
        public ConsoleCommandNotRegistered(string commandName) : base("'" + commandName + "' command is not registered")
        {
            CommandName = commandName;
        }

        public string CommandName { get; private set; }
    }
}