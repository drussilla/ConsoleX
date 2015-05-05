using System;

namespace ConsoleX.UI
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string @string)
        {
            String = @string;
        }

        public string String { get; private set; }
    }
}