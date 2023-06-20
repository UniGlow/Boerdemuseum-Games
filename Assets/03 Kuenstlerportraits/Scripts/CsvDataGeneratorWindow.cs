using System;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Kuenstlerportraits
{
    public class CsvDataGeneratorWindow : EditorWindow
    {

        [MenuItem("Boerde/Portraits/CsvGenerator")]
        static void Init()
        {
            CsvDataGeneratorWindow window = (CsvDataGeneratorWindow)GetWindow(typeof(CsvDataGeneratorWindow));
            window.Show();
        }


        void OnGUI()
        {
            if (GUILayout.Button("GeneratePortraits (Standard)"))
            {
                string relArtistFolderPath = "Assets/03 Kuenstlerportraits/Prefabs/Artists";
                string relImageFolderPath = "Assets/03 Kuenstlerportraits/Sprites/Portraits";
                string csvFile = null;

                try
                {
                    csvFile = EditorUtility.OpenFilePanel("Choose the .csv file.", Application.dataPath, "csv");
                }
                catch (Exception fileException)
                {
                    Debug.LogError(fileException);
                }

                if (string.IsNullOrEmpty(csvFile))
                {
                    Debug.LogError(".csv file not selected.");
                    return;
                }

                if ((!Path.GetExtension(csvFile).Equals(".csv")) && (!Path.GetExtension(csvFile).Equals(".CSV")))
                {
                    Debug.LogError("Unsupported file format.");
                    return;
                }

                GenerateArtistWithPortraits(relArtistFolderPath, relImageFolderPath, csvFile);
            }

            if (GUILayout.Button("GeneratePortraits (Special)"))
            {
                string artistFolder = null;
                string relArtistFolderPath = null;
                string imageFolder = null;
                string relImageFolderPath = null;
                string csvFile = null;

                try
                {
                    artistFolder = EditorUtility.OpenFolderPanel("Choose the folder to save the portraits to.", Application.dataPath, "asset");
                    relArtistFolderPath = artistFolder.Substring(artistFolder.IndexOf("Assets"));
                    imageFolder = EditorUtility.OpenFolderPanel("Choose the folder with the portraits.", Application.dataPath, "asset");
                    relImageFolderPath = imageFolder.Substring(imageFolder.IndexOf("Assets"));
                    csvFile = EditorUtility.OpenFilePanel("Choose the .csv file.", Application.dataPath, "csv");
                }
                catch (Exception fileException)
                {
                    Debug.LogError(fileException);
                }

                if (string.IsNullOrEmpty(artistFolder) || string.IsNullOrEmpty(imageFolder) || string.IsNullOrEmpty(csvFile))
                {
                    Debug.LogError("A folder or file is missing.");
                    return;
                }

                if ((!Path.GetExtension(csvFile).Equals(".csv")) && (!Path.GetExtension(csvFile).Equals(".CSV")))
                {
                    Debug.LogError("Unsupported file format.");
                    return;
                }

                Debug.Log(relArtistFolderPath);
                Debug.Log(relImageFolderPath);

                GenerateArtistWithPortraits(relArtistFolderPath, relImageFolderPath, csvFile);
            }
        }


        void GenerateArtistWithPortraits(string relArtistFolderPath, string relImageFolderPath, string csvFile)
        {
            string csvString = File.ReadAllText(PathExtension.GetAbsolutPath(csvFile, true), System.Text.Encoding.GetEncoding(65001));
            string[] lines = csvString.Split("\n"[0]);
            lines[0] = ";";

            Artist lArtist;
            string lArtistName = lines[1].Trim().Split(";"[0])[0];
            GameObject lGameObject = new GameObject(lArtistName);
            string existingArtist = "";
            try
            {
                existingArtist = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(lArtistName, new string[] { relArtistFolderPath })[0]);
            }
            catch
            {

            }

            Debug.Log(existingArtist);
            if (existingArtist == "")
            {

                lArtist = lGameObject.AddComponent<Artist>();
                lArtist.Name = lArtistName;
            }
            else
            {
                lArtist = AssetDatabase.LoadAssetAtPath(relArtistFolderPath + "/" + lArtistName + ".prefab", typeof(Artist)) as Artist;
                lArtist.Categories.Clear();
            }


            foreach (string line in lines)
            {
                string[] lineData = line.Trim().Split(";"[0]);

                if (lineData.Length == 17)
                {
                    Artwork lArtwork = new Artwork();
                    lArtwork.InvNr = lineData[12];
                    if (lArtwork.SetData(relImageFolderPath))
                    {
                        Category lCategory = lArtist.FindCategory(lineData[13]);

                        if (lCategory == null)
                        {
                            lCategory = new Category();
                            lCategory.Name = lineData[13];
                            lCategory.NameEnglish = lineData[14];
                            lCategory.NamePlatt = lineData[15];

                            lArtist.Categories.Add(lCategory);
                        }

                        if (lineData[4] != "") lArtwork.Year = lineData[4];
                        else lArtwork.Year = "Undatiert";
                        if (lineData[5] != "") lArtwork.YearEnglish = lineData[5];
                        else lArtwork.YearEnglish = "Undated";
                        lArtwork.Technik = lineData[6];
                        lArtwork.Technique = lineData[7];
                        lArtwork.TechnikPlatt = lineData[8];
                        lArtwork.Dimensions = lineData[9];
                        lArtwork.Author = lineData[0];
                        lArtwork.Copyright = lineData[16];
                        lArtwork.Beschreibung = lineData[1];
                        lArtwork.Description = lineData[2];
                        lArtwork.OppPlatt = lineData[3];

                        lCategory.Artworks.Add(lArtwork);
                    }
                    else
                    {
                        Debug.LogError("File missing for table entry: " + lArtwork.InvNr + ". Skipping entry.");
                    }
                }
            }

            if (existingArtist == "")
            {
                PrefabUtility.SaveAsPrefabAsset(lGameObject, relArtistFolderPath + "/" + lArtist.Name + ".prefab");
            }

            DestroyImmediate(lGameObject);
            AssetDatabase.Refresh();

            int entries = 0;
            for (int i = 0; i < lArtist.Categories.Count; i++)
            {
                for (int j = 0; j < lArtist.Categories[i].Artworks.Count; j++)
                {
                    entries++;
                }
            }
            Debug.Log("Successfully created prefab for " + lArtist.Name + " with " + entries + " entries.");
        }
    }
}
#endif