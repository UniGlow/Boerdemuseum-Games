using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap
{
    [RequireComponent(typeof(Image))]
    public class MapSwitcher : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Sprite zoomedOutMap;
        [SerializeField] Sprite zoomedInMap;

        // Private
        Image map;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            map = transform.GetRequiredComponent<Image>();
        }

        private void OnEnable()
        {
            GameEvents.OnZoomingIn += SetZoomedInMap;
            GameEvents.OnZoomingOut += SetZoomedOutMap;
        }

        private void OnDisable()
        {
            GameEvents.OnZoomingIn -= SetZoomedInMap;
            GameEvents.OnZoomedOut -= SetZoomedOutMap;
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void SetZoomedOutMap()
        {
            map.sprite = zoomedOutMap;
        }

        void SetZoomedInMap()
        {
            map.sprite = zoomedInMap;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}