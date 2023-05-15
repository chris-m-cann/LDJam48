using System.Collections;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Util
{
    public class TakeScreenshot : MonoBehaviour
    {
        [SerializeField] 
        private string screenshotPath;

        [SerializeField] private string filename = "screenshot";

        [SerializeField] private float  burstDelay;
        [SerializeField] private float burstGap;
        [SerializeField] private int numberInBurst = 1;
        
        
        [Button]
        public void Screenshot()
        {
            Debug.Log("Taking Screenshot");
            if (!Directory.Exists(screenshotPath)) return;
            
            var path = screenshotPath + Path.DirectorySeparatorChar + filename + ".png";
            if (File.Exists(path))
            {
                int i = 1;

                do
                {
                    path = screenshotPath + Path.DirectorySeparatorChar + $"{filename}-{i}.png";
                    i++;
                } while (File.Exists(path));
            }
            
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"Screenshot {path} taken");
        }

        [Button]
        public void BurstScreenshots()
        {
            StartCoroutine(CoBurstScreenshots());
        }

        private IEnumerator CoBurstScreenshots()
        {
            yield return new WaitForSecondsRealtime(burstDelay);

            for (int i = 0; i < numberInBurst; i++)
            {
                Screenshot();
                yield return new WaitForSecondsRealtime(burstGap);
            }
        }
    }
}