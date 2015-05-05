using System.Collections.Generic;

namespace ConsoleX
{
    public class CommandLineParser : CommandLineParserBase
    {
        private bool isFirstSpaceFound = false;
        private bool isArgumentStarted = false;
        private bool isQuoteOpened = false;
        private int startArgumentIndex = -1;
        
        public override bool TryParse(string commandLine, out string command, out string[] arguments)
        {
            Reset();

            command = null;
            var list = new List<string>();
            arguments = null;

            if (string.IsNullOrEmpty(commandLine))
            {
                return false;
            }

            commandLine = commandLine.Trim();
            var commandEndIndex = commandLine.Length;
            
            for (int i = 0; i < commandLine.Length; i++)
            {
                if (IsFirstSpace(commandLine[i]))
                {
                    commandEndIndex = i;
                    startArgumentIndex = i;
                    isArgumentStarted = true;
                    i++;
                    
                    // last simbol after command is space.
                    if (i >= commandLine.Length)
                    {
                        break;
                    }
                }

                if (isArgumentStarted)
                {
                    if (IsArgumentEnd(commandLine[i]) || i == commandLine.Length - 1)
                    {
                        var arg = commandLine.Substring(startArgumentIndex + 1,
                            i - startArgumentIndex - (i == commandLine.Length - 1 ? 0 : 1));
                        startArgumentIndex = i;

                        // ignore couple spaces in a row
                        if (string.IsNullOrEmpty(arg) || arg == " ")
                        {
                            continue;
                        }

                        arg = arg.Replace("\"", string.Empty);
                        list.Add(arg);
                    }
                }
            }

            command = commandLine.Substring(0, commandEndIndex);
            arguments = list.ToArray();
            return true;
        }

        private bool IsArgumentEnd(char c)
        {
            if (isQuoteOpened && c == '"')
            {
                isQuoteOpened = false;
                return false;
            }

            if (!isQuoteOpened  && c == '"')
            {
                isQuoteOpened = true;
                return false;
            }

            if (isArgumentStarted && !isQuoteOpened && c == ' ')
            {
                return true;
            }

            return false;
        }

        private bool IsFirstSpace(char c)
        {
            if (c == ' ' && !isFirstSpaceFound)
            {
                isFirstSpaceFound = true;
                return true;
            }

            return false;
        }

        private void Reset()
        {
            isFirstSpaceFound = false;
            isArgumentStarted = false;
            isQuoteOpened = false;
            startArgumentIndex = -1;
        }
    }
}