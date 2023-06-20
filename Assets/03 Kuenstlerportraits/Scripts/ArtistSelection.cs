using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Kuenstlerportraits
{
    public class ArtistSelection : MonoBehaviour
    {
        [System.Serializable]
        public class ArtistButton
        {
            public Artist Artist;
            public Button Button;
            public TextMeshProUGUI TextMesh;
        }

        #region Variable Declarations
        public static ArtistSelection Instance;

        // Serialized Fields
        [Header("Animations")]
        [SerializeField] float tweenDuration = 1f;
        [SerializeField] Button selectedArtistOnStart;
        [SerializeField] float fontSizeOnSelection = 42f;

        [Space]
        public List<ArtistButton> artistButtons = new List<ArtistButton>();

        // Private
        ArtistButton selectedArtist;
        float originalFontSize;
        #endregion



        #region Public Properties
        
        #endregion



        #region Unity Event Functions
        private void Start()
        {
            selectedArtist = artistButtons.Find(x => x.Button == selectedArtistOnStart);
            originalFontSize = selectedArtist.TextMesh.fontSize;
            Initialize();
            SelectArtist(selectedArtist.Button);
        }
        #endregion



        #region Public Functions
        public Artist FindArtist(string pName)
        {
            if (artistButtons.Count > 0)
            {
                foreach (ArtistButton artistButton in artistButtons)
                {
                    if (artistButton.Artist.name.Equals(pName)) return artistButton.Artist;
                }
            }

            return null;
        }

        public void SelectArtist(Button button)
        {
            ActivateAllButtons(false);

            ArtistButton newSelectedArtist = artistButtons.Find(x => x.Button == button);

            // Deselect currently selected artist
            selectedArtist.TextMesh.fontStyle = FontStyles.Normal;
            selectedArtist.TextMesh.fontSize = originalFontSize;

            GameEvents.ArtistSelectionStarted(newSelectedArtist.Artist);

            // Selected new artist
            newSelectedArtist.TextMesh.fontStyle = FontStyles.Bold;
            newSelectedArtist.TextMesh.fontSize = fontSizeOnSelection;
            StartCoroutine(Delay(tweenDuration, () => 
            {
                GameEvents.ArtistSelectionFinished(newSelectedArtist.Artist);

                selectedArtist = newSelectedArtist;
                ActivateAllButtons(true);
            }));
        }
        #endregion



        #region Private Functions
        void Initialize()
        {
            foreach (ArtistButton artistButton in artistButtons)
            {
                artistButton.TextMesh.text = artistButton.Artist.Name;
            }
        }

        void ActivateAllButtons(bool active)
        {
            foreach (ArtistButton artistButton in artistButtons)
            {
                artistButton.Button.enabled = active;
            }
        }
        #endregion



        #region Coroutines
        IEnumerator Delay(float seconds, System.Action onComplete)
        {
            yield return new WaitForSeconds(seconds);

            onComplete.Invoke();
        }
        #endregion
    }
}