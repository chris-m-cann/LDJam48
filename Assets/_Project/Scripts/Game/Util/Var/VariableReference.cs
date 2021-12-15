using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var
{
    [Serializable]
    public class VariableReference<TVar, TObVar, T> : OneOf where TVar : Variable<T> where TObVar : ObservableVariable<T>
    {
        [SerializeField] private TVar Variable;
        [SerializeField] private TObVar Observable;
        [SerializeField] private T Constant;

        public virtual T Value
        {
            get
            {
                switch (Delimeter)
                {
                    case 0: return Variable.Value;
                    case 1: return Observable.Value;
                    default: return Constant;
                }
            }

            set
            {
                switch (Delimeter)
                {
                    case 0: Variable.Value = value;
                        return;
                    case 1: Observable.Value = value;
                        return;
                    default: Constant = value;
                        return;
                }
            }
        }

        public VariableReference()
        {
            
        }

        public VariableReference(TVar variable)
        {
            Delimeter = 0;
            Variable = variable;
        }
        
        
        public VariableReference(TObVar observable)
        {
            Delimeter = 1;
            Observable = observable;
        }
        
        public VariableReference(T constant)
        {
            Delimeter = 2;
            Constant = constant;
        }
    }
}