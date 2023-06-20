using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;
using System;

#if UNITY_EDITOR
public class TextureCruncher : EditorWindow
{
    #region Variables

    string imagePath = null;

    int compressionQuality = 75;
    int processingSpeed = 10;

    IEnumerator jobRoutine;
    IEnumerator messageRoutine;

    float progressCount = 0f;
    float totalCount = 1f;

    #endregion



    #region Properties

    float NormalizedProgress
    {
        get { return progressCount / totalCount; }
    }

    float Progress
    {
        get { return progressCount / totalCount * 100f; }
    }

    string FormattedProgress
    {
        get { return Progress.ToString("0.00") + "%"; }
    }

    #endregion


    #region Script Lifecylce

    [MenuItem("Boerde/InterKarte/Texture Cruncher")]
    static void Init()
    {
        var window = (TextureCruncher)EditorWindow.GetWindow(typeof(TextureCruncher));
        window.Show();
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Texture Cruncher", EditorStyles.boldLabel);

        compressionQuality = EditorGUILayout.IntSlider("Compression quality:", compressionQuality, 0, 100);
        processingSpeed = EditorGUILayout.IntSlider("Processing speed:", processingSpeed, 1, 20);

        string buttonLabel = jobRoutine != null ? "Cancel" : "Begin";
        if (GUILayout.Button(buttonLabel))
        {
            if (jobRoutine != null)
            {
                messageRoutine = DisplayMessage("Cancelled. " + FormattedProgress + " complete!", 4f);
                jobRoutine = null;
            }
            else
            {
                try
                {
                    imagePath = EditorUtility.OpenFolderPanel("Choose the folder to load the Images from.", Application.dataPath, Application.dataPath);
                }
                catch (Exception fileException)
                {
                    Debug.LogError(fileException);
                }

                if (string.IsNullOrEmpty(imagePath))
                {
                    return;
                }


                jobRoutine = CrunchTextures();
            }
        }

        if (jobRoutine != null)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(FormattedProgress);

            var rect = EditorGUILayout.GetControlRect();
            rect.width = rect.width * NormalizedProgress;
            GUI.Box(rect, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }
        else if (!string.IsNullOrEmpty(_message))
        {
            EditorGUILayout.HelpBox(_message, MessageType.None);
        }
    }

    void OnEnable()
    {
        EditorApplication.update += HandleCallbackFunction;
    }

    void HandleCallbackFunction()
    {
        if (jobRoutine != null && !jobRoutine.MoveNext())
            jobRoutine = null;


        if (messageRoutine != null && !messageRoutine.MoveNext())
            messageRoutine = null;
    }

    void OnDisable()
    {
        EditorApplication.update -= HandleCallbackFunction;
    }

    #endregion



    #region Logic

    string _message = null;

    IEnumerator DisplayMessage(string message, float duration = 0f)
    {
        if (duration <= 0f || string.IsNullOrEmpty(message))
            goto Exit;

        _message = message;

        while (duration > 0)
        {
            duration -= 0.01667f;
            yield return null;
        }

    Exit:
        _message = string.Empty;
    }

    IEnumerator CrunchTextures()
    {
        DisplayMessage(string.Empty);

        string[] folder = { imagePath.Substring(imagePath.IndexOf("Assets")) };

        var assets = AssetDatabase.FindAssets("t:texture", folder).Select(o => AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(o)) as TextureImporter);
        var eligibleAssets = assets.Where(o => o != null).Where(o => o.compressionQuality != compressionQuality || !o.crunchedCompression);

        totalCount = (float)eligibleAssets.Count();
        progressCount = 0f;

        int quality = compressionQuality;
        int limiter = processingSpeed;
        foreach (var textureImporter in eligibleAssets)
        {
            progressCount += 1f;

            textureImporter.compressionQuality = quality;
            textureImporter.crunchedCompression = true;
            AssetDatabase.ImportAsset(textureImporter.assetPath);

            limiter -= 1;
            if (limiter <= 0)
            {
                yield return null;

                limiter = processingSpeed;
            }
        }

        messageRoutine = DisplayMessage("Crunching complete!", 6f);
        jobRoutine = null;
    }

    #endregion

}
#endif