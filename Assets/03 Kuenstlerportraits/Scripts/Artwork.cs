using UnityEngine;
using UnityEditor;

namespace Kuenstlerportraits
{
    //[CreateAssetMenu(menuName = "Scriptable Objects/Künstlerportraits/Artwork")]
    [System.Serializable]
    public class Artwork// : ScriptableObject
    {

        #region Variable Declarations
        public string Year;
        public string YearEnglish;
        public string Technik;
        public string Technique;
        public string TechnikPlatt;
        public string Dimensions;
        public string InvNr;
        public Sprite Image;
        public string Author;
        public string Copyright;
        [TextArea(2, 4)]
        public string Beschreibung;
        [TextArea(2, 4)]
        public string Description;
        [TextArea(2, 4)]
        public string OppPlatt;
        #endregion



        #region Public Properties

        #endregion



        #region Public Functions
        #if UNITY_EDITOR
        public bool SetData(string imageFolder)
        {
            try
            {
                //Debug.Log(imageFolder + "/" + InvNr + ".jpg");
                Image = AssetDatabase.LoadAssetAtPath<Sprite>(imageFolder + "/" + InvNr + ".jpg");
                if (Image == null) return false;
            }
            catch
            {
                Debug.Log("File " + imageFolder + "/" + InvNr + ".jpg does not exist.");
                return false;
            }

            return true;
        }
        #endif
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}