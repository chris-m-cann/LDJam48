using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util.UI
{
    public class ModelProvider : MonoBehaviour
    {
        [SerializeField] private Model model;
        public event Action<Model> OnModelChanged;

        public Model Model
        {
            get => model;
            set
            {
                model = value;
                OnModelChanged?.Invoke(model);
            }
        }
    }
}