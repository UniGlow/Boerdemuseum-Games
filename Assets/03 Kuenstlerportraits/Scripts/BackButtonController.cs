using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuenstlerportraits
{
    public class BackButtonController : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] float tweenDuration = 1f;

        [Header("References")]
        [SerializeField] GameObject artistDescriptionPanel;
        [SerializeField] GameObject artworksGalleryPanel;
        [SerializeField] GameObject artworkDetailsPanel;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Update()
        {
            if ((Input.GetButtonDown(Constants.INPUT_CANCEL) || Input.GetButtonDown(Constants.INPUT_QUIT)))
            {
                if (!artistDescriptionPanel.activeSelf && artworksGalleryPanel.activeSelf && !artworkDetailsPanel.activeSelf)
                {
                    GameEvents.ArtistSelectionStarted(null);
                    StartCoroutine(Wait(tweenDuration, () => { GameEvents.ArtistSelectionFinished(null); }));
                }
                else if (!artistDescriptionPanel.activeSelf && artworksGalleryPanel.activeSelf && artworkDetailsPanel.activeSelf)
                {
                    GameEvents.ArtworkDescriptionCloseStarted(null);
                    StartCoroutine(Wait(tweenDuration, () => { GameEvents.ArtworkDescriptionCloseFinished(null); }));
                }
            }
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions

        #endregion



        #region Coroutines
        IEnumerator Wait(float seconds, System.Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete.Invoke();
        }
        #endregion
    }
}