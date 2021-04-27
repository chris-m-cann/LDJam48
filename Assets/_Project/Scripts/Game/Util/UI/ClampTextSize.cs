using TMPro;
using UnityEngine;

namespace Util.UI
{
    public class ClampTextSize : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text[] texts;
        [SerializeField] private bool allChildren = true;


        [ContextMenu("ClampSize")]
        public void ClampSize()
        {
            var ts = texts;
            if (allChildren)
            {
                ts = GetComponentsInChildren<TMP_Text>();
            }
            var maxSizeIndex = -1;
            var maxSize = 0;

            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i].text.Length > maxSize)
                {
                    maxSize = ts[i].text.Length;
                    maxSizeIndex = i;
                }
            }

            if (maxSizeIndex == -1) return;

            var textSize = ts[maxSizeIndex].fontSize;
            for (int i = 0; i < ts.Length; i++)
            {
                if (i == maxSize) continue;

                ts[i].enableAutoSizing = false;
                ts[i].fontSize = textSize;
            }
        }

        [ContextMenu("ResetAutoSizing")]
        public void ResetAutoSizing()
        {
            var ts = texts;
            if (allChildren)
            {
                ts = GetComponentsInChildren<TMP_Text>();
            }

            foreach (var text in ts)
            {
                text.enableAutoSizing = true;
            }
        }
    }
}
