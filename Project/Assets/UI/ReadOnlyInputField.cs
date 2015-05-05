using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConsoleX.UI
{
    public class ReadOnlyInputField : InputField
    {
        public event EventHandler OnCharEntered = delegate { };
        public event EventHandler<StringEventArgs> OnValueInserted = delegate { }; 
        /// <summary>
        /// Handle the specified event.
        /// </summary>
        private Event m_ProcessingEvent = new Event();

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (!isFocused)
                return;

            bool consumedEvent = false;
            while (Event.PopEvent(m_ProcessingEvent))
            {
                if (m_ProcessingEvent.rawType == EventType.KeyDown)
                {
                    if (!IsAllowedCombination(m_ProcessingEvent))
                    {
                        OnCharEntered(this, new EventArgs());
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
            {
                UpdateLabel();
            }

            eventData.Use();
        }

        private bool IsAllowedCombination(Event evt)
        {
            var currentEventModifiers = evt.modifiers;
            RuntimePlatform rp = Application.platform;
            bool isMac = (rp == RuntimePlatform.OSXEditor || rp == RuntimePlatform.OSXPlayer || rp == RuntimePlatform.OSXWebPlayer);
            bool ctrl = isMac ? (currentEventModifiers & EventModifiers.Command) != 0 : (currentEventModifiers & EventModifiers.Control) != 0;
            
            switch (evt.keyCode)
            {
                case KeyCode.Home:
                case KeyCode.End:
                case KeyCode.LeftControl:
                case KeyCode.RightControl:
                {
                    return true;
                }

                // Select All
                case KeyCode.A:
                {
                    if (ctrl)
                    {
                        return true;
                    }
                    break;
                }

                // Copy
                case KeyCode.C:
                {
                    if (ctrl)
                    {
                        return true;
                    }
                    break;
                }

                case KeyCode.V:
                {
                    if (ctrl)
                    {
                        OnValueInserted(this, new StringEventArgs(GetClipboard()));
                        return false;
                    }
                    break;
                }

                case KeyCode.LeftArrow:
                case KeyCode.RightArrow:
                case KeyCode.UpArrow:
                case KeyCode.DownArrow:
                {
                    return true;
                }
            }

            return false;
        }

        private string GetClipboard()
        {
            TextEditor te = new TextEditor();
            te.Paste();
            return te.content.text;
        }
    }
}