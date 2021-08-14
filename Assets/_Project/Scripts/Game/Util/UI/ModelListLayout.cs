using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
    public class ModelListLayout : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private ModelProvider entryPrefab;
        [SerializeField] private Model[] models;
        private Transform _Container
        {
            get
            {
                container = container ?? transform;
                return container;
            }
        }

        private int _initialChildCount = 0;

        private void Awake()
        {
            _initialChildCount = _Container.childCount;
            BuildLayout();
        }

        private void BuildLayout()
        {
            DestroyChildren();
            
            foreach (var model in models)
            {
                var provider = Instantiate(entryPrefab, container, false);
                provider.Model = model;
            }

            // todo(chris) this is STILL not ensuring the object is layout out properly when first built!!
            RefreshLayout();
        }

        public void RefreshLayout()
        {
            var group = _Container.gameObject.GetComponent<LayoutGroup>();
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
            _Container.gameObject.SetActive(false);
            _Container.gameObject.SetActive(true);
        }

        public void DestroyChildren()
        {
            for (int i = container.childCount - 1; i >= _initialChildCount; --i)
            {
                Destroy(container.GetChild(i));
            }
        }
    }
}