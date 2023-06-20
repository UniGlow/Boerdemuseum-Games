using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveMap
{
    /// <summary>
    /// This class stores all GameEvents and provides functions to call them.
    /// </summary>
    /// <remarks>
    /// Move this class from the plugins into the project assets to be able to reference game-specific datatypes (Plugins folder is comiled in "firstpass").
    /// </remarks>
    public class GameEvents
    {
        public delegate void ZoomHandler();
        public static event ZoomHandler OnZoomedIn;
        public static void ZoomedIn()
        {
            Debug.Log("Invoking GameEvent \"OnZoomedIn\".");
            OnZoomedIn?.Invoke();
        }
        public static event ZoomHandler OnZoomingOut;
        public static void ZoomingOut()
        {
            Debug.Log("Invoking GameEvent \"OnZoomingOut\".");
            OnZoomingOut.Invoke();
        }
        public static event ZoomHandler OnZoomedOut;
        public static void ZoomedOut()
        {
            Debug.Log("Invoking GameEvent \"OnZoomedOut\".");
            OnZoomedOut?.Invoke();
        }

        public static event ZoomHandler OnZoomingIn;
        public static void ZoomingIn()
        {
            Debug.Log("Invoking GameEvent \"OnPoiGroupClicked\".");
            OnZoomingIn?.Invoke();
        }

        /*public delegate void PoiClickedHandler(DataSet dataSet);
        public static event PoiClickedHandler OnPoiClicked;

        public static void PoiClicked(DataSet dataSet)
        {
            Debug.Log("Invoking GameEvent \"OnPoiClicked\".");
            OnPoiClicked?.Invoke(dataSet);
        }*/

        public delegate void PoiGroupClickedHandler(List<POI> poiList);
        public static event PoiGroupClickedHandler OnPoiGroupClicked;

        public static void PoiGroupClicked(List<POI> poiList)
        {
            Debug.Log("Invoking GameEvent \"OnPoiGroupClicked\".");
            OnPoiGroupClicked?.Invoke(poiList);
        }

        public delegate void PoiDescriptionHandler();
        public static event PoiDescriptionHandler OnPoiDescriptionClosed;
        public static void PoiDescriptionClosed()
        {
            Debug.Log("Invoking GameEvent \"OnPoiDescriptionClosed\".");
            OnPoiDescriptionClosed?.Invoke();
        }
    }
}