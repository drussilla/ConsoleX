using System;

namespace ConsoleX.Exceptions
{
    public class CommandLineParseException : Exception
    {
        public CommandLineParseException(string message) : base(message)
        {
        }
    }
}