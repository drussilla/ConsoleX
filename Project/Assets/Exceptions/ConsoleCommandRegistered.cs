using System;

namespace ConsoleX.Exceptions
{
    public class ConsoleCommandAlreadyRegistered : Exception
    {
        public ConsoleCommandAlreadyRegistered(string commandName) : base("'" + commandName + "' command is already registered.")
        {
        }
    }
}