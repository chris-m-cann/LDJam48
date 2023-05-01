using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ConditionalEventBehaviour : MonoBehaviour
    {
        public abstract class Condition
        {
            public abstract bool Check();
        }

        public class PlatformCondition : Condition
        {
            public UnityEngine.RuntimePlatform targetPlatform;

            public override bool Check()
            {
                return Application.platform == targetPlatform;
            }
        }
        
        [Serializable]
        public struct Case
        {
            [SerializeReference] public Condition Condition;
            public UnityEvent OnTrue;
        }

        [SerializeField] private Case[] cases = new Case[0];
        [SerializeField] private UnityEvent defaultCase;
        

        public void Evaluate()
        {
            for (int i = 0; i < cases.Length; i++)
            {
                if (cases[i].Condition.Check())
                {
                    cases[i].OnTrue?.Invoke();
                    return;
                }
            }
            
            defaultCase?.Invoke();
        }
    }
}