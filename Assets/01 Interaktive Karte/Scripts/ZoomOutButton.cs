using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap
{
    public class ZoomOutButton : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Sprite zoomInImage;
        [SerializeField] Sprite zoomOutImage;
        [SerializeField] bool showOnStart = false;

        // Private
        Image image;
        Button button;
        bool isScrollZoomBlocked = false;
        #endregion



        #region Public Properties

        #endregion

        

        #region Unity Event Functions
        private void Awake()
        {
            image = transform.GetRequiredComponent<Image>();
            button = transform.GetRequiredComponent<Button>();

            if (showOnStart) ZoomingIn();
            else ZoomingOut();
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y == 0 || isScrollZoomBlocked) return;

            if (Input.mouseScrollDelta.y > 0 && image.sprite != zoomOutImage)
            {
                GameEvents.ZoomingIn();
            }
            else if (Input.mouseScrollDelta.y < 0 && image.sprite != zoomInImage)
            {
                GameEvents.ZoomingOut();
            }
        }

        private void OnEnable()
        {
            GameEvents.OnZoomingIn += ZoomingIn;
            GameEvents.OnZoomingOut += ZoomingOut;
            GameEvents.OnPoiGroupClicked += BlockScrollZoom;
        }

        private void OnDisable()
        {
            GameEvents.OnZoomingIn -= ZoomingIn;
            GameEvents.OnZoomingOut -= ZoomingOut;
            GameEvents.OnPoiGroupClicked -= BlockScrollZoom;
        }
        #endregion



        #region Public Functions
        public void ChangeZoom()
        {
            if(image.sprite == zoomInImage) GameEvents.ZoomingIn();
            else GameEvents.ZoomingOut();
        }

        public void UnblockScrollZoom()
        {
            isScrollZoomBlocked = false;
        }
        #endregion



        #region Private Functions
        void ZoomingIn()
        {
            image.sprite = zoomOutImage;
        }

        void ZoomingOut()
        {
            image.sprite = zoomInImage;
        }

        void BlockScrollZoom(List<POI> pPoiList)
        {
            isScrollZoomBlocked = true;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}