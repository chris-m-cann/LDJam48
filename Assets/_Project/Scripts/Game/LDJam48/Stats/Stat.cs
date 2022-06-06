using System;
using UnityEngine;
using Util;
using Util.UI;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48.Stats
{
    public abstract class Stat : Model
    {
        public string DisplayName;
        public abstract void Register();
        public abstract void Unregister();
        
        public abstract string ValueDisplayString();
        
        
        public abstract void Reset();
        
        // for some reason when putting this directly on the virtual method it isnt trigger so have an editor specific one
        [ContextMenu("ResetStat")]
        private void EditorReset() => Reset();

    }

    public abstract class StatT<T> : Stat
    {
        public virtual T Value { get; set; }
    }
    
    public abstract class EventDrivenStat<TEvent, TObservable, T> : StatT<T> where TEvent : EventReferenceBase<T> where TObservable : ObservableVariable<T>
    {
        
        [SerializeField] protected TEvent @event;
        [Nested] public TObservable Current;
        [SerializeField] protected T resetValue;

        public override T Value { get => Current.Value; set => Current.Value = value; }

        protected abstract void OnUpdate(T value);
        
        public override void Register()
        {
            @event.OnEventTriggered += OnUpdate;
        }

        public override void Unregister()
        {
            @event.OnEventTriggered -= OnUpdate;
        }

        public override string ValueDisplayString() => Current.Value.ToString();

        public override void Reset()
        {
            Current.Value = resetValue;
        }
        
        private void OnEnable()
        {
            if (String.IsNullOrEmpty(DisplayName))
            {
                DisplayName = name;
            }
        }
    }
}