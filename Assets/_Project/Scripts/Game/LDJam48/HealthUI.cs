using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    [RequireComponent(typeof(Image))]
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable health;
        [SerializeField] private IntReference healthPerLife;
        [SerializeField] private float flashTime = .2f;
        
        
        private Image _image;
        private Material _mat;
        private Color _c;
        private static readonly int Fill = Shader.PropertyToID("Fill");

        private void Awake()
        {
            _image = GetComponent<Image>();
            _mat = _image.material;
            _c = _image.color;
            UpdateHealth(0);
        }

        private void OnEnable()
        {
            health.OnValueChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            health.OnValueChanged -= UpdateHealth;
        }

        private void UpdateHealth(int v)
        {
            var normalisedV = (float)v / healthPerLife.Value;
            _mat.SetFloat(Fill, normalisedV);
            _image.color = Color.white;
            this.ExecuteAfterUnscaled(flashTime, ()=> _image.color = _c);
        }
    }
}