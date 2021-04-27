using TMPro;
using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(TMP_Text))]
    public class SetFormattedText : MonoBehaviour
    {
        [SerializeField] private string format;


        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void SetText(int v) =>_text.text = string.Format(format, v);
        public void SetText(float v) =>_text.text = string.Format(format, v);
        public void SetText(string v) =>_text.text = string.Format(format, v);
        public void SetText(char v) =>_text.text = string.Format(format, v);
    }
}