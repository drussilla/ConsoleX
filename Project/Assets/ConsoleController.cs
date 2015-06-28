using System;
using ConsoleX.Exceptions;
using ConsoleX.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConsoleX
{
    public class ConsoleController : MonoBehaviour
    {
        [SerializeField]
        private GameObject consolePanel;
        [SerializeField]
        private ReadOnlyInputField output;
        [SerializeField]
        private CommandPrompt commandPrompt;
        [SerializeField]
        private Console console;

        public Console Console
        {
            get { return console; }
        }

        // Use this for initialization
        private void Start()
        {
            commandPrompt.OnExecuteCommand+= CommandPromptOnOnExecuteCommand;
            commandPrompt.OnCompleteCommand += CommandPromptOnOnCompleteCommand;
            output.OnCharEntered += OutputOnCharEntered;
            output.OnValueInserted += OutputOnOnValueInserted;
        }

        private void Update()
        {
            if (isPromptNeedUpdate)
            {
                EventSystem.current.SetSelectedGameObject(commandPrompt.gameObject);
                commandPrompt.text = newValueForPrompt;
                commandPrompt.MoveCaretToEnd();
                isPromptNeedUpdate = false;
            }
        }

        private void OnDestroy()
        {
            commandPrompt.OnExecuteCommand -= CommandPromptOnOnExecuteCommand;
            commandPrompt.OnCompleteCommand -= CommandPromptOnOnCompleteCommand;
            output.OnCharEntered -= OutputOnCharEntered;
            output.OnValueInserted -= OutputOnOnValueInserted;
        }

        private void CommandPromptOnOnCompleteCommand(object sender, CommandPromptEventArgs commandPromptEventArgs)
        {
            var completeItems = Console.CompleteCommandLine(commandPromptEventArgs.CommandLine);
            if (completeItems.Length == 0)
            {
                // do nothing
            }
            else if (completeItems.Length == 1)
            {
                // no arguments in command prompt
                if (commandPromptEventArgs.CommandLine.IndexOf(" ", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    // full command already entered. Add space at the end.
                    if (completeItems[0].Length == commandPromptEventArgs.CommandLine.Length)
                    {
                        UpdateCommandPrompt(completeItems[0] + " ");
                    }
                    else // otherwise just replace with commnad (might be different case)
                    {
                        UpdateCommandPrompt(commandPrompt.text = completeItems[0]);
                    }
                }
                else // arguments was in the prompt
                {
                    // just add text. Command auto complete should decide what to return
                    UpdateCommandPrompt(commandPrompt.text + completeItems[0]);
                }
            }
            else
            {
                output.text += string.Join("\n", completeItems);
                output.MoveCaretToEnd();
            }
        }

        private string newValueForPrompt;
        private bool isPromptNeedUpdate = false;
        
        private void UpdateCommandPrompt(string value)
        {
            EventSystem.current.SetSelectedGameObject(output.gameObject);
            newValueForPrompt = value;
            isPromptNeedUpdate = true;
        }

        public bool IsVisible { get; private set; }

        public void Show()
        {
            consolePanel.SetActive(true);
            commandPrompt.DisableInputForFrames(1);
            EventSystem.current.SetSelectedGameObject(commandPrompt.gameObject);
            IsVisible = true;
        }

        public void Hide()
        {
            consolePanel.SetActive(false);
            IsVisible = false;
        }

        private void OutputOnOnValueInserted(object sender, StringEventArgs stringEventArgs)
        {
            commandPrompt.text += stringEventArgs.String.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

        void OutputOnCharEntered(object sender, EventArgs e)
        {
            EventSystem.current.SetSelectedGameObject(commandPrompt.gameObject);
        }

        private void CommandPromptOnOnExecuteCommand(object sender, CommandPromptEventArgs eventArgs)
        {
            try
            {
                Console.ExecuteCommandLine(eventArgs.CommandLine);
            }
            catch (ConsoleCommandNotRegistered ex)
            {
                output.text += "\n" + ex.CommandName + " is not registered";
            }
        }
    }
}
