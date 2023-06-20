using UnityEngine;
using UnityEditor;

namespace InteractiveMap
{
    [System.Serializable]
    public class DataSet
    {
        public string Author;
        public string Description;
        public string DescriptionEnglish;
        public string DescriptionPlatt;
        public string Date;
        public string DateEnglish;
        public string Technique;
        public string TechniqueEnglish;
        public string TechniquePlatt;
        public string Dimensions;
        public string LatiAndLongitude;
        public GPSCoords GpsCoords = new GPSCoords();
        public string FileName;
        public Sprite Image;
        public string Copyright;
        public string Radius;
        public AudioClip Audio;
        public bool isAudio;

#if UNITY_EDITOR
        public bool SetData(string dataPath, bool pIsAudio)
        {
            //Latitude
            GpsCoords.latitude.degrees = float.Parse(LatiAndLongitude.Substring(0, 2), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            GpsCoords.latitude.minutes = float.Parse(LatiAndLongitude.Substring(3, 2), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            GpsCoords.latitude.seconds = float.Parse(LatiAndLongitude.Substring(6, 4), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            //Longitude
            GpsCoords.longitude.degrees = float.Parse(LatiAndLongitude.Substring(13, 2), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            GpsCoords.longitude.minutes = float.Parse(LatiAndLongitude.Substring(16, 2), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            GpsCoords.longitude.seconds = float.Parse(LatiAndLongitude.Substring(19, 4), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            isAudio = pIsAudio;

            if (isAudio)
            {
                try
                {
                    Audio = AssetDatabase.LoadAssetAtPath<AudioClip>(dataPath.Substring(dataPath.IndexOf("Assets")) + "/" + FileName + ".mp3");
                    if (Audio == null) return false;
                }
                catch
                {
                    Debug.Log("File " + dataPath + "/" + FileName + ".mp3 does not exist.");
                    return false;
                }
            }
            else
            {
                try
                {
                    Image = AssetDatabase.LoadAssetAtPath<Sprite>(dataPath.Substring(dataPath.IndexOf("Assets")) + "/" + FileName + ".jpg");
                    if (Image == null) return false;
                }
                catch
                {
                    Debug.Log("File " + dataPath + "/" + FileName + ".jpg does not exist.");
                    return false;
                }
            }

            return true;
        }
#endif
    }
}