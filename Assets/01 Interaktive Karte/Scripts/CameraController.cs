using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace InteractiveMap
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : UniGlow.CameraController
    {

        #region Variable Declarations

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            GameEvents.OnZoomingIn += ZoomingIn;
            GameEvents.OnZoomingOut += ZoomOut;
        }

        private void OnDisable()
        {
            GameEvents.OnZoomingIn -= ZoomingIn;
            GameEvents.OnZoomingOut -= ZoomOut;
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void ZoomingIn()
        {
            ZoomIn(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y));
        }
        #endregion



        #region Coroutines

        #endregion
    }
}