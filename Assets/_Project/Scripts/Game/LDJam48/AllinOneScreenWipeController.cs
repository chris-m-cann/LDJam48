using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Scenes;
using Util.Var.Events;


/*
 * on scene ended just use the image set
 * on scene load, or on image set, whatever happens first then start wipe out
 * dont set image to null if not your image
 * 
 *
 * 
 */

namespace LDJam48
{
    public class AllinOneScreenWipeController : SceneChangeHandler
    {
        [SerializeField] private ScreenWipe[] screenWipes;
        [SerializeField] private ImageGameEvent setImage;
        [SerializeField] private ImageGameEvent clearImage;
        [SerializeField] private int test = -1;
        [SerializeField] private float covered = 0;
        [SerializeField] private float uncovered = 1.1f;

        private ScrabbleBag<ScreenWipe> _wipes;
        private Material _mat;
        private Image _image;
        private ScreenWipe _current;
        private bool _sceneEnded = false;
        private static readonly int Cutoff = Shader.PropertyToID("Cutoff");

        private void Awake()
        {
            _wipes = new ScrabbleBag<ScreenWipe>(screenWipes, true);
        }

        private void OnEnable()
        {
            setImage.OnEventTrigger += OnSetImage;
            clearImage.OnEventTrigger += OnClearImage;
        }

        private void OnDisable()
        {
            setImage.OnEventTrigger  -= OnSetImage;
            clearImage.OnEventTrigger  -= OnClearImage;
        }

        private void OnSetImage(Image image)
        {
            Debug.Log($"{name}: image changed: {_sceneEnded}");
            if (image == null) return;
            
            _image = image;
            _mat = image.material;
            if (_sceneEnded)
            {
                _mat.SetFloat(Cutoff, 0);
                // StopAllCoroutines();
                StartCoroutine(CoWipeIn());
            }
        }
        
        private void OnClearImage(Image image)
        {
            Debug.Log($"{name}: image cleared: {_sceneEnded}, == _image:{_image == image}");
            if (_image == image)
            {
                _image = null;
                _mat = null;
            }
        }

        public override IEnumerator CoSceneLoaded()
        {
            yield break;
        }

        private IEnumerator CoWipeIn()
        {
            _sceneEnded = false;
            yield return StartCoroutine(CoTween(covered, uncovered, _current.WipeInTime, _current.EffectImage));

                        
            if (_image == null) yield break; 
            _image.gameObject.SetActive(false);
        }
        
        private IEnumerator CoTween(float from, float to, float duration, Sprite sprite)
        {
            Debug.Log($"CoTween from {from} to {to}: duration {duration}, _mat == null:{_mat == null}, _image == null:{_image == null}");
            if (_mat == null) yield break;
            if (_image == null) yield break; 
            _image.sprite = sprite;
            _image.gameObject.SetActive(true);

            SetCutoff(from);

            float start = Time.unscaledTime;
            float end = start + duration;

            while (_mat != null && Time.unscaledTime < end)
            {
                var t = (Time.unscaledTime - start) / duration;
                var f = from + (to - from) * Mathf.Clamp01(t);
                SetCutoff(f);
                yield return null;
            }

            if (_mat != null)
            {
                SetCutoff(to);
            }

            yield return null;
        }

        private void SetCutoff(float f)
        {
            _mat.SetFloat(Cutoff, f);
        }

        public override IEnumerator CoSceneEnding(string currentScenePath, string nextScenePath)
        {
            _sceneEnded = true;
            _current = (test >= 0) ? screenWipes[test] : _wipes.GetRandomElement();
            yield return StartCoroutine(CoTween(uncovered, covered, _current.WipeOutTime, _current.EffectImage));
        }
    }
}