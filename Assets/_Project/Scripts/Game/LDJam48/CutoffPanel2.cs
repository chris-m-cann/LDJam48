using System;
using System.Collections;
using LDJam48.Var.Observe;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class CutoffPanel2 : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ImageGameEvent setImage;
        [SerializeField] private ImageGameEvent clearImage;
        [SerializeField] private BoolReference setupOnAwake ;


        private void Awake()
        {
            Debug.Log($"{name}: COP Awake");
            if (setupOnAwake.Value)
            {
                Debug.Log($"{name}: Setting cutoff");
                image.material.SetFloat("Cutoff", 0);
            }
        }

        private void Start()
        {
            Debug.Log($"{name}: COP Start");
            if (setupOnAwake.Value)
            {
                Debug.Log($"{name}: Setting value");
                setImage.Raise(image);
            }
        }

        private void OnDisable()
        {
            clearImage.Raise(image);
        }
    }
}