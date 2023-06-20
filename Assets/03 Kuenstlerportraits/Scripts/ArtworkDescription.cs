using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Kuenstlerportraits
{
    public class ArtworkDescription : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Header("Animations")]
        [SerializeField] float tweenDuration = 1f;
        [SerializeField] Ease tweeningEase = Ease.InOutCubic;
        [SerializeField] Vector2 tweenOutOfScreenPosition;

        [Header("References")]
        [SerializeField] GameObject artworkDescriptionPanel;
        [SerializeField] Image image;
        [SerializeField] UniGlow.MultiLanguageText titleTextMesh;
        [SerializeField] UniGlow.MultiLanguageText date;
        [SerializeField] UniGlow.MultiLanguageText technique;
        [SerializeField] UniGlow.MultiLanguageText dimensions;
        [SerializeField] UniGlow.MultiLanguageText invnr;

        [Header("Copyright")]
        [SerializeField] TextMeshProUGUI copyright;
        [Tooltip("Maximal y offset of the copyright text when the sprite ratio is 1,5.")]
        [SerializeField] float yOffsetMax;
        [Tooltip("Maximal y offset of the copyright text when the sprite ratio is 0,75.")]
        [SerializeField] float xOffsetMax;

        // Private
        Artwork displayedArtwork;
        Category currentCategory;
        int currentArtworkNo;
        RectTransform rectTransform;
        Vector2 originalAnchoredPosition;
        Vector3 copyrightOriginalPosition = new Vector3(-36, 18, 0);
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rectTransform = artworkDescriptionPanel.transform.GetRequiredComponent<RectTransform>();
            originalAnchoredPosition = rectTransform.anchoredPosition;
            artworkDescriptionPanel.SetActive(false);
            copyrightOriginalPosition = copyright.rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            GameEvents.OnArtworkSelectionStarted += Show;
            GameEvents.OnArtistSelectionStarted += Hide;
            GameEvents.OnCategorySelectionStarted += Hide;
            GameEvents.OnArtworkDescriptionCloseStarted += Hide;
        }

        private void OnDisable()
        {
            GameEvents.OnArtworkSelectionStarted -= Show;
            GameEvents.OnArtistSelectionStarted -= Hide;
            GameEvents.OnCategorySelectionStarted -= Hide;
            GameEvents.OnArtworkDescriptionCloseStarted -= Hide;
        }
        #endregion



        #region Public Functions
        public void Next(bool right)
        {
            if (!right && currentArtworkNo % 9 == 0) GameEvents.LoadPreviousArtwork();
            int lArtworksCount = currentCategory.Artworks.Count;
            currentArtworkNo = right ? (currentArtworkNo + 1) % lArtworksCount : ((currentArtworkNo - 1) % lArtworksCount + lArtworksCount) % lArtworksCount;
            Show(currentCategory.Artworks[currentArtworkNo], currentCategory, currentArtworkNo);
            if (right && currentArtworkNo % 9 == 0) GameEvents.LoadNextArtwork();
        }

        public void Exit()
        {
            GameEvents.ArtworkDescriptionCloseStarted(displayedArtwork);
        }
        #endregion



        #region Private Functions
        void Show(Artwork artwork, Category category, int artworkNo)
        {
            image.sprite = artwork.Image;
            currentCategory = category;
            currentArtworkNo = artworkNo;
            titleTextMesh.SetTexts(artwork.Beschreibung, artwork.Description, artwork.OppPlatt);
            date.SetTexts(artwork.Year, artwork.YearEnglish, artwork.Year);
            technique.SetTexts(artwork.Technik, artwork.Technique, artwork.TechnikPlatt);
            dimensions.SetTexts(artwork.Dimensions, artwork.Dimensions, artwork.Dimensions);
            invnr.SetTexts(artwork.InvNr, artwork.InvNr, artwork.InvNr);

            displayedArtwork = artwork;
            artworkDescriptionPanel.SetActive(true);

            copyright.text = "© " + artwork.Copyright;

            //adjust copyright position
            if (artwork.Image != null)
            {
                float spriteRatio = artwork.Image.bounds.size.y / artwork.Image.bounds.size.x; //-> <1 = breiter, >1 = höher
                float percentage = 0.7f / spriteRatio; //-> >1 = muss hoch, <1 muss links
                if (percentage > 1)
                {
                    float yPosition = copyrightOriginalPosition.y + 360 * (1 - (1 / percentage));
                    yPosition = Mathf.Clamp(yPosition, 0f, yOffsetMax);
                    copyright.rectTransform.anchoredPosition = new Vector3(copyrightOriginalPosition.x, yPosition, copyrightOriginalPosition.z);
                }
                else
                {
                    float xPosition = copyrightOriginalPosition.x - 510 * (1 - percentage);
                    xPosition = Mathf.Clamp(xPosition, xOffsetMax, 0f);
                    copyright.rectTransform.anchoredPosition = new Vector3(xPosition, copyrightOriginalPosition.y, copyrightOriginalPosition.z);
                }
            }

            if (rectTransform.anchoredPosition == originalAnchoredPosition) return;

            rectTransform.anchoredPosition = tweenOutOfScreenPosition;
            rectTransform.DOAnchorPos(originalAnchoredPosition, tweenDuration).SetEase(tweeningEase);
        }

        void Hide()
        {
            rectTransform.DOAnchorPos(tweenOutOfScreenPosition, tweenDuration).SetEase(tweeningEase).OnComplete(() =>
            {
                artworkDescriptionPanel.SetActive(false);
                GameEvents.ArtworkDescriptionCloseFinished(displayedArtwork);
            });
        }

        void Hide(Artwork artwork) { Hide(); }
        void Hide(Category category) { Hide(); }
        void Hide(Artist artist) { Hide(); }
        #endregion



        #region Coroutines

        #endregion
    }
}