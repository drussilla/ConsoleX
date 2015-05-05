using System;

namespace ConsoleX.UI
{
    public class CommandPromptEventArgs : EventArgs
    {
        public CommandPromptEventArgs(string commandLine)
        {
            CommandLine = commandLine;
        }

        public string CommandLine { get; private set; }
    }
}