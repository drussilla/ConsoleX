using System;
using System.Collections;
using UnityEngine;

namespace ConsoleX.Helpers
{
    public static class CoroutineHelper
    {
        public static IEnumerator DoActionAfterFrames(this MonoBehaviour target, Action action, int frames)
        {
            yield return target.StartCoroutine(WaitFor.Frames(frames));
            action();
        }
    }
}