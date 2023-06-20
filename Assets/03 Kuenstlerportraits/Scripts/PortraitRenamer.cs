using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Kuenstlerportraits
{
    public class PortraitRenamer : EditorWindow
    {

        [MenuItem("Boerde/Portraits/PortraitRenamer")]
        static void Init()
        {
            PortraitRenamer window = (PortraitRenamer)EditorWindow.GetWindow(typeof(PortraitRenamer));
            window.Show();
        }


        void OnGUI()
        {
            if (GUILayout.Button("RenamePortraits"))
            {
                string renamePath = null;

                try
                {
                    renamePath = EditorUtility.OpenFolderPanel("Choose the Folder for Renaming.", Application.dataPath, "asset");
                }
                catch
                {
                    Debug.LogError("Could not select Folder");
                }
                
                if (string.IsNullOrEmpty(renamePath))
                {
                    return;
                }


                foreach (Texture2D item in LoadAllAssetsAtPath(renamePath.Substring(renamePath.IndexOf("Assets"))))
                {
                    AssetDatabase.RenameAsset(renamePath.Substring(renamePath.IndexOf("Assets")) + "/" + item.name + ".jpg", item.name.Substring(item.name.IndexOf("V_")));
                }
                AssetDatabase.Refresh();
            }
        }

        List<Object> LoadAllAssetsAtPath(string path)
        {
            List<Object> objects = new List<Object>();
            if (Directory.Exists(path))
            {
                string[] assets = Directory.GetFiles(path);
                foreach (string assetPath in assets)
                {
                    if (assetPath.Contains(".jpg") && assetPath.Contains("_V_") && !assetPath.Contains(".meta"))
                    {
                        objects.Add(AssetDatabase.LoadMainAssetAtPath(assetPath));
                    }
                }
            }
            return objects;
        }
    }
}
#endif