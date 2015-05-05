using System;

namespace ConsoleX
{
    public interface IConsole
    {
        void RegisterCommand(string commandName, Action<string[]> commandAction, Func<string, string[]> autoComplete = null);
        bool TryRegisterCommand(string commandName, Action<string[]> commandAction, Func<string, string[]> autoComplete = null);

        void RegisterCommand(IConsoleCommand command);
        bool TryRegisterCommand(IConsoleCommand command);

        void RegisterAsyncCommand(string commandName, Action<string[]> commandAction, Func<bool> isFinished, Func<string, string[]> autoComplete = null);
        bool TryRegisterAsyncCommand(string commandName, Action<string[]> commandAction, Func<bool> isFinished, Func<string, string[]> autoComplete = null);

        bool IsRegistered(string commandName);
        bool IsRegistered(IConsoleCommand command);

        string[] CompleteCommandLine(string commandLine);

        void ExecuteCommandLine(string commandLine);

        void ExecuteCommand(string command, string[] arguments);

        void DeregisterCommand(string commandName);
        bool TryDeregisterCommand(string commandName);

        void DeregisterCommand(IConsoleCommand command);
        bool TryDeregisterCommand(IConsoleCommand command);
    }
}
