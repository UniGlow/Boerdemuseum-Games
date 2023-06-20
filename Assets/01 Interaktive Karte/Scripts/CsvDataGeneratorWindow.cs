using System;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace InteractiveMap
{
    public class CsvDataGeneratorWindow : EditorWindow
    {

        [MenuItem("Boerde/InterKarte/CsvGenerator")]
        static void Init()
        {
            CsvDataGeneratorWindow window = (CsvDataGeneratorWindow)GetWindow(typeof(CsvDataGeneratorWindow));
            window.Show();
        }


        private void OnGUI()
        {
            if (GUILayout.Button("GenerateImagePOIs (General)"))
            {
                string savePath = null;
                string imagePath = null;
                string csvFile = null;

                try
                {
                    savePath = EditorUtility.OpenFolderPanel("Choose the folder to save the POIs.", Application.dataPath, Application.dataPath);
                    imagePath = EditorUtility.OpenFolderPanel("Choose the folder to load the Images from.", Application.dataPath, Application.dataPath);
                    csvFile = EditorUtility.OpenFilePanel("Choose a .csv file.", Application.dataPath, "csv");
                }
                catch (Exception fileException)
                {
                    Debug.LogError(fileException);
                }

                GeneratePOIs(savePath, imagePath, csvFile, false);
            }

            if (GUILayout.Button("GenerateAudioPOIs (General)"))
            {
                string savePath = null;
                string audioPath = null;
                string csvFile = null;

                try
                {
                    savePath = EditorUtility.OpenFolderPanel("Choose the folder to save the POIs.", Application.dataPath, Application.dataPath);
                    audioPath = EditorUtility.OpenFolderPanel("Choose the folder to load the Audios from.", Application.dataPath, Application.dataPath);
                    csvFile = EditorUtility.OpenFilePanel("Choose a .csv file.", Application.dataPath, "csv");
                }
                catch (Exception fileException)
                {
                    Debug.LogError(fileException);
                }

                GeneratePOIs(savePath, audioPath, csvFile, true);
            }

            if (GUILayout.Button("GeneratePhotoPOIs"))
            {
                string savePath = "Assets/01 Interaktive Karte/Scriptable Objects";
                string photoPath = "Assets/01 Interaktive Karte/Sprites/Fotos verkleinert";

                GeneratePOIs(savePath, photoPath, GetCSVFile(), false);
            }

            if (GUILayout.Button("GenerateArtworkPOIs"))
            {
                string savePath = "Assets/01 Interaktive Karte/Scriptable Objects";
                string artworkPath = "Assets/01 Interaktive Karte/Sprites/Kunst";

                GeneratePOIs(savePath, artworkPath, GetCSVFile(), false);
            }

            if (GUILayout.Button("GenerateRecordingPOIs"))
            {
                string savePath = "Assets/01 Interaktive Karte/Scriptable Objects";
                string recordingPath = "Assets/01 Interaktive Karte/Audio/Tonaufnahmen";

                GeneratePOIs(savePath, recordingPath, GetCSVFile(), true);
            }
        }


        string GetCSVFile()
        {
            try
            {
                return EditorUtility.OpenFilePanel("Choose a .csv file.", Application.dataPath, "csv");
            }
            catch (Exception fileException)
            {
                Debug.LogError(fileException);
                return "";
            }
        }


        void GeneratePOIs(string savePath, string filePath, string csvFile, bool isAudio)
        {
            if (string.IsNullOrEmpty(savePath) || string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(csvFile))
            {
                Debug.LogError("A folder or file is missing.");
                return;
            }

            if ((!Path.GetExtension(csvFile).Equals(".csv")) && (!Path.GetExtension(csvFile).Equals(".CSV")))
            {
                Debug.LogError("Unsupported file format");
                return;
            }

            string csvString = File.ReadAllText(PathExtension.GetAbsolutPath(csvFile, true), System.Text.Encoding.GetEncoding(65001)); //UTF7);  //GetEncoding(65001));
            string[] lines = csvString.Split("\n"[0]);
            lines[0] = ";;";
            DataSetList dataSetList = CreateInstance<DataSetList>();

            foreach (string line in lines)
            {
                string[] lineData = line.Trim().Split(";"[0]);

                if (lineData.Length == 17)
                {
                    DataSet dataSet = new DataSet();

                    dataSet.LatiAndLongitude = lineData[10];
                    dataSet.FileName = lineData[12];
                    if (dataSet.SetData(filePath, isAudio))
                    {
                        dataSet.Author = lineData[0];
                        dataSet.Description = lineData[1];
                        dataSet.DescriptionEnglish = lineData[2];
                        dataSet.DescriptionPlatt = lineData[3];
                        if (lineData[4] != "") dataSet.Date = lineData[4];
                        else dataSet.Date = "Undatiert";
                        if (lineData[5] != "") dataSet.DateEnglish = lineData[5];
                        else dataSet.DateEnglish = "Undated";
                        dataSet.Technique = lineData[6];
                        dataSet.TechniqueEnglish = lineData[7];
                        dataSet.TechniquePlatt = lineData[8];
                        if (!isAudio) dataSet.Dimensions = lineData[9];
                        dataSet.Radius = lineData[11];
                        dataSet.Copyright = lineData[16];

                        dataSetList.Sets.Add(dataSet);
                    }
                    else
                    {
                        Debug.LogError("File missing for table entry: " + dataSet.FileName + ". Skipping entry.");
                    }
                }
            }

            if (isAudio) AssetDatabase.CreateAsset(dataSetList, PathExtension.GetAssetDirectory(savePath, true) + "NewAudioDataSetList.asset");
            else AssetDatabase.CreateAsset(dataSetList, PathExtension.GetAssetDirectory(savePath, true) + "NewImageDataSetList.asset");
            AssetDatabase.SaveAssets();

            Debug.Log("Successfully created list with " + dataSetList.Sets.Count + " entries.");
        }
    }
}
#endif