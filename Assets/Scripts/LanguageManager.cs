using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow
{
    public class LanguageManager : MonoBehaviour
    {

        #region Variable Declarations
        public static string CurrentLanguage = Constants.LANGUAGE_GERMAN;

        // Serialized Fields

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            GameEvents.OnLanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            GameEvents.OnLanguageChanged -= ChangeLanguage;
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void ChangeLanguage(string language)
        {
            CurrentLanguage = language;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}