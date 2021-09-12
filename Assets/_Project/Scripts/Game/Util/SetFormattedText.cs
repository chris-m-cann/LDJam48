using TMPro;
using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace Util
{
    [RequireComponent(typeof(TMP_Text))]
    public class SetFormattedText : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string format = "{0}";

        
        private TMP_Text _text;

        private TMP_Text _Text
        {
            get
            {
                if (_text == null)
                {
                    _text = GetComponent<TMP_Text>();
                }

                return _text;
            }
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void SetText(int v) =>_Text.text = string.Format(format, v);
        public void SetText(float v) =>_Text.text = string.Format(format, v);
        public void SetText(string v) =>_Text.text = string.Format(format, v);
        public void SetText(char v) =>_Text.text = string.Format(format, v);
        public void SetText(IntVariable v) =>_Text.text = string.Format(format, v.Value);
        public void SetText(StringVariable v) =>_Text.text = string.Format(format, v.Value);
        public void SetText(FloatVariable v) =>_Text.text = string.Format(format, v.Value);
        
        public void SetText(ObservableIntVariable v) =>_Text.text = string.Format(format, v.Value);
        public void SetText(ObservableStringVariable v) =>_Text.text = string.Format(format, v.Value);
        public void SetText(ObservableFloatVariable v) =>_Text.text = string.Format(format, v.Value);
    }
}