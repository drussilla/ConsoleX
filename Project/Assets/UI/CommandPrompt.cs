using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConsoleX.UI
{
    public class CommandPrompt : InputField
    {
        public event EventHandler<CommandPromptEventArgs> OnCompleteCommand = delegate { };
        public event EventHandler<CommandPromptEventArgs> OnExecuteCommand = delegate { };
        public event EventHandler<CommandPromptEventArgs> OnPreviousCommand = delegate { };
        public event EventHandler<CommandPromptEventArgs> OnNextCommand = delegate { };
        public event EventHandler<CommandPromptEventArgs> OnClearPrompt = delegate { };

        /// <summary>
        /// Handle the specified event.
        /// </summary>
        private Event m_ProcessingEvent = new Event();

        public void MoveCaretToEnd()
        {
            m_CaretPosition = text.Length;
            m_CaretSelectPosition = text.Length;
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (!isFocused)
                return;

            bool consumedEvent = false;
            while (Event.PopEvent(m_ProcessingEvent))
            {
                if (m_ProcessingEvent.rawType == EventType.KeyDown)
                {
                    if (ProcessKeyPress(m_ProcessingEvent))
                    {
                        continue;
                    }
                    consumedEvent = true;
                    
                    var shouldContinue = KeyPressed(m_ProcessingEvent);
                    if (shouldContinue == EditState.Finish)
                    {
                        DeactivateInputField();
                        break;
                    }
                }
            }

            if (consumedEvent)
                UpdateLabel();

            eventData.Use();
        }

        private bool ProcessKeyPress(Event evt)
        {
            switch (evt.keyCode)
            {
                case KeyCode.UpArrow:
                {
                    OnPreviousCommand(this, new CommandPromptEventArgs(text));
                    return true;
                }

                case KeyCode.DownArrow:
                {
                    OnNextCommand(this, new CommandPromptEventArgs(text));
                    return true;
                }

                case KeyCode.Tab:
                {
                    OnCompleteCommand(this, new CommandPromptEventArgs(text));
                    return true;
                }

                case KeyCode.KeypadEnter:
                case KeyCode.Return:
                {
                    OnExecuteCommand(this, new CommandPromptEventArgs(text));
                    text = "";
                    return true;
                }

                case KeyCode.Escape:
                {
                    OnClearPrompt(this, new CommandPromptEventArgs(text));
                    text = "";
                    return true;
                }
            }

            if (evt.character == 10)
            {
                return true;
            }

            return false;
        }
    }
}