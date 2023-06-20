using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveMap
{
    [CreateAssetMenu(menuName = "Scriptable Objects/MapSettings")]
    public class MapSettings : ScriptableObject
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Vector2 minMaxLatitude; // y
        [SerializeField] Vector2 minMaxLongitude; // x



        // Private

        #endregion



        #region Public Properties
        public Vector2 MinMaxLatitude { get => minMaxLatitude; }
        public Vector2 MinMaxLongitude { get => minMaxLongitude; }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}