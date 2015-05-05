using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ConsoleX;
using ConsoleX.Exceptions;

namespace ConsoleX
{
    public class Console : MonoBehaviour, IConsole
    {
        private List<IConsoleCommand> registeredCommands;
        private Queue<ConsoleCommandInstance> commandsQueue;
        private ConsoleCommandInstance currentCommand;

        [SerializeField]
        private CommandLineParser commandLineParse;

        public CommandLineParser CommandLineParser { get { return commandLineParse; } }
        #region Unity Flow

        // Use this for initialization
        public void Awake()
        {
            registeredCommands = new List<IConsoleCommand>();
            commandsQueue = new Queue<ConsoleCommandInstance>();

            if (commandLineParse == null)
            {
                commandLineParse = gameObject.AddComponent<CommandLineParser>();
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (currentCommand == null && commandsQueue.Count > 0)
            {
                // todo: add multy threading
                currentCommand = commandsQueue.Dequeue();
                currentCommand.Command.Action(currentCommand.Arguments);
                currentCommand = null;
            }
        }

        #endregion

        #region RegisterCommand

        public void RegisterCommand(string commandName, Action<string[]> commandAction,
            Func<string, string[]> autoComplete = null)
        {
            if (IsRegistered(commandName))
            {
                throw new ConsoleCommandAlreadyRegistered(commandName);
            }

            RegisterCommandSafe(commandName, commandAction, autoComplete);
        }

        public bool TryRegisterCommand(string commandName, Action<string[]> commandAction,
            Func<string, string[]> autoComplete = null)
        {
            if (IsRegistered(commandName))
            {
                return false;
            }

            RegisterCommandSafe(commandName, commandAction, autoComplete);
            return true;
        }

        public void RegisterCommand(IConsoleCommand command)
        {
            if (IsRegistered(command.Name))
            {
                throw new ConsoleCommandAlreadyRegistered(command.Name);
            }

            registeredCommands.Add(command);
        }

        public bool TryRegisterCommand(IConsoleCommand command)
        {
            if (IsRegistered(command.Name))
            {
                return false;
            }

            registeredCommands.Add(command);
            return true;
        }

        public void RegisterAsyncCommand(string commandName, Action<string[]> commandAction, Func<bool> isFinished,
            Func<string, string[]> autoComplete = null)
        {
            if (IsRegistered(commandName))
            {
                throw new ConsoleCommandAlreadyRegistered(commandName);
            }

            RegisterAsyncCommandSafe(commandName, commandAction, isFinished, autoComplete);
        }

        public bool TryRegisterAsyncCommand(string commandName, Action<string[]> commandAction, Func<bool> isFinished,
            Func<string, string[]> autoComplete = null)
        {
            if (IsRegistered(commandName))
            {
                return false;
            }

            RegisterAsyncCommandSafe(commandName, commandAction, isFinished, autoComplete);
            return true;
        }

        #endregion

        public bool IsRegistered(string commandName)
        {
            return registeredCommands.FindIndex(x => x.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase)) !=
                   -1;
        }

        public bool IsRegistered(IConsoleCommand command)
        {
            return IsRegistered(command.Name);
        }

        public string[] CompleteCommandLine(string commandLine)
        {
            commandLine = commandLine.TrimStart();

            var spaceIndex = commandLine.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
            // command with argumens
            if (spaceIndex != -1)
            {
                // extract command and args
                var commandString = commandLine.Substring(0, spaceIndex);
                if (!IsRegistered(commandString))
                {
                    return new string[0];
                }

                var command = Get(commandString);
                if (command.AutoComplete == null)
                {
                    return new string[0];
                }

                var arguments = commandLine.Substring(spaceIndex, commandLine.Length - spaceIndex).Trim();

                return command.AutoComplete(arguments);
            }

            var items =
                registeredCommands
                    .Where(x => x.Name.StartsWith(commandLine, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Name)
                    .ToArray();
            return items;
        }

        public void ExecuteCommand(string command, string[] arguments)
        {
            IConsoleCommand consoleCommand;
            if (TryGet(command, out consoleCommand))
            {
                var commandToExecute = new ConsoleCommandInstance(this, consoleCommand, arguments);

                // todo: add multy threading
                commandsQueue.Enqueue(commandToExecute);
            }
        }

        public void ExecuteCommandLine(string commandLine)
        {
            string command;
            string[] arguments;
            if (!commandLineParse.TryParse(commandLine, out command, out arguments))
            {
                throw new CommandLineParseException("Cannot parse command line");
            }

            if (IsRegistered(command))
            {
                ExecuteCommand(command, arguments);
                return;
            }

            throw new ConsoleCommandNotRegistered(command);
        }

        public void DeregisterCommand(string commandName)
        {
            IConsoleCommand command;
            if (TryGet(commandName, out command))
            {
                registeredCommands.Remove(command);
            }
            else
            {
                throw new ConsoleCommandNotRegistered(commandName);
            }
        }

        public bool TryDeregisterCommand(string commandName)
        {
            IConsoleCommand command;
            if (TryGet(commandName, out command))
            {
                registeredCommands.Remove(command);
                return true;
            }

            return false;
        }

        public void DeregisterCommand(IConsoleCommand command)
        {
            DeregisterCommand(command.Name);
        }

        public bool TryDeregisterCommand(IConsoleCommand command)
        {
            return TryDeregisterCommand(command.Name);
        }

        private void RegisterCommandSafe(string commandName, Action<string[]> commandAction,
            Func<string, string[]> autoComplete)
        {
            IConsoleCommand command =
                autoComplete == null
                    ? new ConsoleCommand(commandName, commandAction)
                    : new ConsoleCommand(commandName, commandAction, autoComplete);

            registeredCommands.Add(command);
        }

        private void RegisterAsyncCommandSafe(string commandName, Action<string[]> commandAction, Func<bool> isFinished,
            Func<string, string[]> autoComplete)
        {
            IConsoleCommand command =
                autoComplete == null
                    ? new ConsoleCommand(commandName, commandAction, isFinished)
                    : new ConsoleCommand(commandName, commandAction, isFinished, autoComplete);
            registeredCommands.Add(command);
        }

        private IConsoleCommand Get(string commandName)
        {
            return registeredCommands.Find(x => x.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
        }

        private bool TryGet(string commandName, out IConsoleCommand command)
        {
            command = Get(commandName);
            return command != null;
        }
    }
}
