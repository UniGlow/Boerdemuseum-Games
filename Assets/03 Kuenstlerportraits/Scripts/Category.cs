using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuenstlerportraits
{
    //[CreateAssetMenu(menuName = "Scriptable Objects/Künstlerportraits/Category")]
    [System.Serializable]
    public class Category// : ScriptableObject
    {

        #region Variable Declarations
        public string Name;
        public string NameEnglish;
        public string NamePlatt;
        public List<Artwork> Artworks = new List<Artwork>();
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            Artworks = new List<Artwork>();
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}