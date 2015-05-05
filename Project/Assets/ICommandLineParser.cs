using UnityEngine;

namespace ConsoleX
{
    public abstract class CommandLineParserBase : MonoBehaviour
    {
        public abstract bool TryParse(string commandLine, out string command, out string[] arguments);
    }
}