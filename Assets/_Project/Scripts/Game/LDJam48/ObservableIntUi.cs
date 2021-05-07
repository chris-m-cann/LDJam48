using System;
using TMPro;
using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{

    [RequireComponent(typeof(TMP_Text))]
    public class ObservableIntUi : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable value;
        [SerializeField] private string format;


        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            SetText(value.Value);
        }

        private void OnEnable()
        {
            value.OnValueChanged += SetText;
        }

        private void OnDisable()
        {
            value.OnValueChanged -= SetText;
        }

        private void SetText(int v)
        {
            _text.text = string.Format(format, v);
        }


    }
}