using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Kuenstlerportraits
{
    public class ArtworksGallery : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Header("Animations")]
        [SerializeField] float tweenDuration = 1f;
        [SerializeField] Ease tweeningEase = Ease.InOutCubic;
        [SerializeField] Vector2 tweenOutOfScreenPosition;

        [Header("References")]
        [SerializeField] GameObject artworksGallery;
        [SerializeField] List<ArtworkDisplayer> artworkDisplays = new List<ArtworkDisplayer>();
        [SerializeField] TextMeshProUGUI pageNumberTextMesh;
        [SerializeField] List<GameObject> buttons = new List<GameObject>();

        // Private
        RectTransform rectTransform;
        Vector2 originalAnchoredPosition;
        Category currentlySelectedCategory;
        int currentPage;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rectTransform = artworksGallery.transform.GetRequiredComponent<RectTransform>();
            originalAnchoredPosition = rectTransform.anchoredPosition;

            Hide();
        }

        private void OnEnable()
        {
            GameEvents.OnCategorySelectionFinished += Show;
            GameEvents.OnArtistSelectionStarted += Hide;
            GameEvents.OnCategorySelectionStarted += Hide;
            GameEvents.OnLoadNextArtwork += ShowNextArtworks;
            GameEvents.OnLoadPreviousArtwork += ShowPreviousArtworks;

        }

        private void OnDisable()
        {
            GameEvents.OnCategorySelectionFinished -= Show;
            GameEvents.OnArtistSelectionStarted -= Hide;
            GameEvents.OnCategorySelectionStarted -= Hide;
            GameEvents.OnLoadNextArtwork -= ShowNextArtworks;
            GameEvents.OnLoadPreviousArtwork -= ShowPreviousArtworks;
        }
        #endregion



        #region Public Functions
        public void ShowNextArtworks()
        {
            // Only show next page if more artworks are available
            if (currentlySelectedCategory.Artworks.Count <= (currentPage + 1) * 9) currentPage = 0;
            else currentPage++;

            foreach (ArtworkDisplayer artworkDisplay in artworkDisplays)
            {
                artworkDisplay.FillImage(currentlySelectedCategory, currentPage);
            }

            UpdatePageNumber();
        }

        public void ShowPreviousArtworks()
        {
            // Never go below 0
            if (currentPage == 0) currentPage = currentlySelectedCategory.Artworks.Count / 9;
            else currentPage--;

            foreach (ArtworkDisplayer artworkDisplay in artworkDisplays)
            {
                artworkDisplay.FillImage(currentlySelectedCategory, currentPage);
            }

            UpdatePageNumber();
        }
        #endregion



        #region Private Functions
        void Show(Category category)
        {
            artworksGallery.SetActive(true);

            if ((((category.Artworks.Count - 1) / 9) + 1) > 1)
            {
                foreach (GameObject button in buttons)
                {
                    button.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject button in buttons)
                {
                    button.SetActive(false);
                }
            }

            foreach (ArtworkDisplayer artworkDisplay in artworkDisplays)
            {
                artworkDisplay.FillImage(category, currentPage);
            }

            rectTransform.anchoredPosition = tweenOutOfScreenPosition;
            rectTransform.DOAnchorPos(originalAnchoredPosition, tweenDuration).SetEase(tweeningEase);

            currentlySelectedCategory = category;

            UpdatePageNumber();
        }

        void Hide()
        {
            rectTransform.DOAnchorPos(tweenOutOfScreenPosition, tweenDuration).SetEase(tweeningEase).OnComplete(() =>
            {
                artworksGallery.SetActive(false);
                currentPage = 0;
            });
        }

        void Hide(Artist artist)
        {
            Hide();
        }

        void Hide(Category category)
        {
            Hide();
        }

        void UpdatePageNumber()
        {
            pageNumberTextMesh.text = (currentPage + 1).ToString() + " / " + (((currentlySelectedCategory.Artworks.Count - 1) / 9) + 1).ToString();
        }
        #endregion



        #region Coroutines

        #endregion
    }
}