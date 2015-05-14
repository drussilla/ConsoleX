using System;
using System.Collections;

namespace ConsoleX.Helpers
{
    public static class WaitFor
    {
        public static IEnumerator Frames(int frameCount)
        {
            if (frameCount <= 0)
            {
                throw new ArgumentOutOfRangeException("frameCount", "Cannot wait less that 1 frame");
            }

            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }
}