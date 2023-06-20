using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace InteractiveMap
{
    public class CrunchCompressable : EditorWindow
    {

        [MenuItem("Boerde/InterKarte/CrunchCompressable")]
        static void Init()
        {
            CrunchCompressable window = (CrunchCompressable)EditorWindow.GetWindow(typeof(CrunchCompressable));
            window.Show();
        }


        private void OnGUI()
        {
            if (GUILayout.Button("JPGsForDTX1"))
            {
                string imagePath = null;

                try
                {
                    imagePath = EditorUtility.OpenFolderPanel("Choose the folder to load the Images from.", Application.dataPath, Application.dataPath);
                }
                catch
                {
                    Debug.LogError("Could not load.");
                }

                if (string.IsNullOrEmpty(imagePath))
                {
                    return;
                }

                CutAllAssetsAtPath(imagePath.Substring(imagePath.IndexOf("Assets")));
            }
        }


        void CutAllAssetsAtPath(string path)
        {
            if (Directory.Exists(path))
            {
                string[] assets = Directory.GetFiles(path);
                foreach (string assetPath in assets)
                {
                    if (!assetPath.Contains(".meta") && (assetPath.Contains(".jpg") || assetPath.Contains(".JPG")))
                    {
                        Texture2D tex = (Texture2D)AssetDatabase.LoadMainAssetAtPath(assetPath);
                        if (tex.width % 4 != 0 || tex.height % 4 != 0)
                        {
                            File.WriteAllBytes(assetPath.Substring(assetPath.IndexOf("Assets")), WriteArrayToTexture(tex).EncodeToJPG());
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
        }


        public static Texture2D WriteArrayToTexture(Texture2D tex)
        {
            int widthDest = tex.width - tex.width % 4;
            int heightDest = tex.height - tex.height % 4;
            Texture2D newTex = new Texture2D(widthDest, heightDest);

            for (int i = 0; i < widthDest; i++)
            {
                for (int j = 0; j < heightDest; j++)
                {
                    newTex.SetPixel(i, j, tex.GetPixel(i, j));
                }
            }

            newTex.Apply();

            return newTex;
        }
    }
}
#endif