using System;
using System.Globalization;
using System.IO;
using LDJam48;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    [InitializeOnLoad]
    public static class ScreenshotSceneView
    {
        private static string LEVELS_ASSET_FOLDER = "_Project/Prefabs/Levels/LevelChunks/";
        private static string SCREENSHOT_ASSET_FOLDER = "_Project/Prefabs/Levels/ScreenShots/";

        private static int width = 300;
        private static int height = 300;
        private static int cropLeft = 100;
        private static int cropRight = 100;
        private static int cropTop = 50;
        private static int cropBottom = 50;
        private static bool ensureTransparentBackground = false;

        
        static ScreenshotSceneView()
        {
            // PrefabStage.prefabStageClosing += OnPrefabStageClosing;
            // very heavy way to do this but should work
            // PrefabStage.prefabSaved += OnPrefabStageSaved;
            LevelChunk.OnScreenshotRequest += OnPrefabStageSaved;
        }

        static void OnPrefabStageClosing(PrefabStage prefabStage, bool refreshAssetDbAfter = true)
        {
            Debug.Log($"Closing Level: {prefabStage.name}, assetPath = {prefabStage.assetPath}");
            bool b = prefabStage.assetPath.StartsWith("Assets/" + LEVELS_ASSET_FOLDER, true, CultureInfo.InvariantCulture);

            if (b)
            {
                Debug.Log($"Closing Level: {prefabStage.prefabContentsRoot.name}");
                TakeScreenshot(prefabStage.prefabContentsRoot.GetComponent<LevelChunk>(), prefabStage.assetPath, refreshAssetDbAfter);
            }
        }
        
        static void OnPrefabStageSaved(LevelChunk prefab)
        {
            var o = EditorUtility.GetPrefabParent(prefab);
            string assetPath = AssetDatabase.GetAssetPath(o);
            bool b = assetPath.StartsWith("Assets/" + LEVELS_ASSET_FOLDER, true, CultureInfo.InvariantCulture);

            if (b)
            {
                TakeScreenshot(prefab, assetPath);
            }
        }
        
        
        private static void Screenshot()
        {
            // Get actvive EditorWindow
            var activeWindow = EditorWindow.focusedWindow;

            // Get screen position and sizes
            var vec2Position = activeWindow.position.position;
            var sizeX = activeWindow.position.width;
            var sizeY = activeWindow.position.height;

            // Take Screenshot at given position sizes
            var colors = InternalEditorUtility.ReadScreenPixel(vec2Position, (int)sizeX, (int)sizeY);

            // write result Color[] data into a temporal Texture2D
            var result = new Texture2D((int)sizeX, (int)sizeY);
            result.SetPixels(colors);

            // encode the Texture2D to a PNG
            // you might want to change this to JPG for way less file size but slightly worse quality
            // if you do don't forget to also change the file extension below
            var bytes = result.EncodeToPNG();

            // In order to avoid bloading Texture2D into memory destroy it
            Object.DestroyImmediate(result);

            // finally write the file e.g. to the StreamingAssets folder
            var timestamp = System.DateTime.Now;
            var stampString = string.Format("_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}", timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second);
            File.WriteAllBytes(Path.Combine(Application.dataPath, "Screenshot" + stampString + ".png"), bytes);

            // Refresh the AssetsDatabase so the file actually appears in Unity
            AssetDatabase.Refresh();

            Debug.Log("New Screenshot taken");
        }

        
        public static void TakeScreenshot(LevelChunk prefab, string prefabAssetPath, bool refreshAssetDbAfter = true)
        {
            
            Camera cam = SceneView.lastActiveSceneView.camera;
            

            // Create Render Texture with width and height.
            RenderTexture rt = new RenderTexture(width + cropLeft + cropRight, height + cropBottom + cropTop, 0, RenderTextureFormat.ARGB32);

            // Assign Render Texture to camera.
            cam.targetTexture = rt;

            // save current background settings of the camera
            CameraClearFlags clearFlags = cam.clearFlags;
            Color backgroundColor = cam.backgroundColor;

            // make the background transparent when enabled
            if (ensureTransparentBackground)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(); // alpha is zero
            }

            // Render the camera's view to the Target Texture.
            cam.Render();

            // restore the camera's background settings if they were changed before rendering
            if (ensureTransparentBackground)
            {
                cam.clearFlags = clearFlags;
                cam.backgroundColor = backgroundColor;
            }

            // Save the currently active Render Texture so we can override it.
            RenderTexture currentRT = RenderTexture.active;

            // ReadPixels reads from the active Render Texture.
            RenderTexture.active = cam.targetTexture;

            // Make a new texture and read the active Render Texture into it.
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
            screenshot.ReadPixels(new Rect(cropLeft, cropBottom,  width, height), 0, 0, false);

            // PNGs should be sRGB so convert to sRGB color space when rendering in linear.
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                Color[] pixels = screenshot.GetPixels();
                for (int p = 0; p < pixels.Length; p++)
                {
                    pixels[p] = pixels[p].gamma;
                }

                screenshot.SetPixels(pixels);
            }

            // Apply the changes to the screenshot texture.
            screenshot.Apply(false);

            var nestedFolders = prefabAssetPath.Replace("Assets/" + LEVELS_ASSET_FOLDER, "");
            int i = nestedFolders.LastIndexOf('/');
            if (i != -1)
            {
                nestedFolders = nestedFolders.Substring(0, i + 1);
            }
            else
            {
                nestedFolders = "";
            }
            var filenamePrefix = $"{prefab.Intensity}_Screenshot_{prefab.name}";
            string dir = Application.dataPath + "/" + SCREENSHOT_ASSET_FOLDER + nestedFolders;
            string filename = filenamePrefix + ".png";
            string path = dir + filename;
            // Save the screenshot.
            Directory.CreateDirectory(dir);
            byte[] png = screenshot.EncodeToPNG();
            File.WriteAllBytes(path, png);

            // Remove the reference to the Target Texture so our Render Texture is garbage collected.
            cam.targetTexture = null;

            // Replace the original active Render Texture.
            RenderTexture.active = currentRT;

            if (refreshAssetDbAfter)
            {
                // Refresh the AssetsDatabase so the file actually appears in Unity
                AssetDatabase.Refresh();
            }

            Debug.Log("Screenshot saved to:\n" + path);
        }

        public static string GetSafePath(string path)
        {
            return string.Join("_", path.Split(Path.GetInvalidPathChars()));
        }

        public static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }


        [MenuItem("Tools/ScreenshotAllLevels")]
        private static void ScreenshotAllLevels()
        {
            ForeachForldersAndFiles(Application.dataPath + "/" + LEVELS_ASSET_FOLDER);
            
            // Refresh the AssetsDatabase so all the file actually appears in Unity
            AssetDatabase.Refresh();
        }
        
        private static void ForeachForldersAndFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] arrDir = di.GetDirectories();

            foreach (DirectoryInfo dir in arrDir)
            {
                ForeachForldersAndFiles(dir.ToString() + "/");
            }

            foreach (FileInfo fi in di.GetFiles("*.*"))
            {
                // Debug.Log($"fi.name {fi.Name}, fullname {fi.FullName}, extenstion {fi.Extension}, tostring {fi.ToString()}");
                if (fi.Extension == ".prefab")
                {

                    string assetPath = fi.FullName.Replace("\\", "/");
                    assetPath = assetPath.Replace(Application.dataPath, "Assets");
                    
                    
                    // Debug.Log($"fi.name {fi.Name}, assetpath = {assetPath}");
                    
                    PrefabStage stage = PrefabStageUtility.OpenPrefab(assetPath);
                    if (stage != null)
                    {
                        if (stage.prefabContentsRoot.GetComponent<LevelChunk>() != null)
                        {
                            OnPrefabStageClosing(stage, false);
                        }
                    }
                }
            }
        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            string name = EditorUtility.InstanceIDToObject(instanceID).name;

            if (assetPath.StartsWith("Assets/" + SCREENSHOT_ASSET_FOLDER))
            {
                Debug.Log("Open Asset step: 1 (" + name + ")");

                string endBit = assetPath.Replace("Assets/" + SCREENSHOT_ASSET_FOLDER, "");
                int i = endBit.LastIndexOf('/');
                if (i != -1)
                {
                    endBit = endBit.Substring(0, i + 1);
                }
                else
                {
                    endBit = "";
                }

                i = name.IndexOf("Screenshot_", StringComparison.Ordinal);
                i = i < 0 ? 0 : i;
                string prefabName = name.Substring(i + 11); 

                string newAssetPath = "Assets/" + LEVELS_ASSET_FOLDER + endBit + prefabName + ".prefab";

                PrefabStage ps = PrefabStageUtility.OpenPrefab(newAssetPath);

                return ps != null;
            }

            return false; // we did not handle the open
        }
        
    }
}