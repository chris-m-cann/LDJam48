using System;
using System.Collections;
using LDJam48.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public static class CoroutinesEx
    {

        public static void ExecuteAfter(this MonoBehaviour self, float delay, UnityAction action) {
            self.StartCoroutine(CoExecuteAfter(delay, action, true));
        }

        public static void ExecuteAfterUnscaled(this MonoBehaviour self, float delay, UnityAction action)
        {
            self.StartCoroutine(CoExecuteAfter(delay, action, false));
        }

        private static IEnumerator CoExecuteAfter(float delay, UnityAction action, bool useTimeScale)
        {
            if (useTimeScale)
            {
                yield return new WaitForSeconds(delay);
            } else
            {
                yield return new WaitForSecondsRealtime(delay);
            }

            action();
        }

        public static void ExecuteNextFrame(this MonoBehaviour self, UnityAction action)
        {
            self.StartCoroutine(CoExecuteNextFrame(action));
        }

        private static IEnumerator CoExecuteNextFrame(UnityAction action)
        {
            yield return null;
            action();
        }
    }
}
