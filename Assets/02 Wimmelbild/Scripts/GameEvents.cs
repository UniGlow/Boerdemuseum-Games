using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wimmelbild
{
    /// <summary>
    /// This class stores all GameEvents and provides functions to call them.
    /// </summary>
    /// <remarks>
    /// Move this class from the plugins into the project assets to be able to reference game-specific datatypes (Plugins folder is comiled in "firstpass").
    /// </remarks>
    public class GameEvents
    {
        public delegate void SearchableFoundHandler(Searchable searchable);
        public static event SearchableFoundHandler OnSearchableKlicked;
        public static void SearchableKlicked(Searchable searchable)
        {
            Debug.Log("Invoking GameEvent: \"OnSearchableKlicked\" for searchable: " + searchable + ".", searchable);
            OnSearchableKlicked?.Invoke(searchable);
        }

        public static event SearchableFoundHandler OnSearchableFound;
        public static void SearchableFound(Searchable searchable)
        {
            Debug.Log("Invoking GameEvent: \"OnSearchableFound\" for searchable: " + searchable + ".", searchable);
            OnSearchableFound?.Invoke(searchable);
        }
    }
}