using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow
{
    /// <summary>
    /// This class stores all GameEvents and provides functions to call them.
    /// </summary>
    /// <remarks>
    /// Move this class from the plugins into the project assets to be able to reference game-specific datatypes (Plugins folder is comiled in "firstpass").
    /// </remarks>
    public class GameEvents
    {
        public delegate void LanguageHandler(string language);
        public static event LanguageHandler OnLanguageChanged;
        public static void LanguageChanged(string language)
        {
            Debug.Log("Invoking GameEvent \"OnLanguageChanged\" for language: " + language + ".");
            OnLanguageChanged?.Invoke(language);
        }
    }
}