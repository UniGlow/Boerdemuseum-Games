using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Kuenstlerportraits
{
    public class ArtworkDisplayer : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Space]
        [SerializeField] int id;

        [Header("References")]
        [SerializeField] RectTransform mouseOverPanel;
        [SerializeField] Image image;
        [SerializeField] Image background;
        [SerializeField] UniGlow.MultiLanguageText titleTextMesh;
        [SerializeField] TextMeshProUGUI yearTextMesh;
        [SerializeField] Button button;

        // Private
        Artwork displayedArtwork;
        Category currentCategory;
        int currentArtworkNo;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            GameEvents.OnArtworkSelectionStarted += DeactivateButton;
            GameEvents.OnArtworkDescriptionCloseFinished += ActivateButton;
        }

        private void OnDisable()
        {
            GameEvents.OnArtworkSelectionStarted -= DeactivateButton;
            GameEvents.OnArtworkDescriptionCloseFinished -= ActivateButton;
        }
        #endregion



        #region Public Functions
        public void FillImage(Category pCategory, int page)
        {
            if (pCategory.Artworks.Count > id + page * 9)
            {
                currentCategory = pCategory;
                currentArtworkNo = id + page * 9;
                FillImage(currentCategory.Artworks[currentArtworkNo]);
                Show();
            }
            else Hide();
        }

        public void ShowArtwork()
        {
            GameEvents.ArtworkSelectionStarted(displayedArtwork, currentCategory, currentArtworkNo);
        }

        public void ShowMouseOver()
        {
            mouseOverPanel.gameObject.SetActive(true);
        }

        public void HideMouseOver()
        {
            mouseOverPanel.gameObject.SetActive(false);
        }
        #endregion



        #region Private Functions
        void FillImage(Artwork artwork)
        {
            ShowMouseOver();
            image.sprite = artwork.Image;
            titleTextMesh.SetTexts(artwork.Beschreibung, artwork.Description, artwork.OppPlatt);
            yearTextMesh.text = artwork.Year;

            displayedArtwork = artwork;
            HideMouseOver();
        }

        void Show()
        {
            image.enabled = true;
            background.enabled = true;
            titleTextMesh.enabled = true;
            yearTextMesh.enabled = true;
            ActivateButton();
        }

        void Hide()
        {
            image.enabled = false;
            background.enabled = false;
            titleTextMesh.enabled = false;
            yearTextMesh.enabled = false;
        }

        void ActivateButton(Artwork artwork = null)
        {
            button.enabled = true;
        }

        void DeactivateButton(Artwork artwork = null, Category category = null, int artworkNo = 0)
        {
            button.enabled = false;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}