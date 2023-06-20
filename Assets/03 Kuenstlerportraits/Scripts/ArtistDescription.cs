using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Kuenstlerportraits
{
    public class ArtistDescription : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Header("Animations")]
        [SerializeField] float tweenDuration = 1f;
        [SerializeField] Ease tweeningEase = Ease.InOutCubic;
        [SerializeField] Vector2 tweenOutOfScreenPosition;

        [Header("References")]
        [SerializeField] GameObject artistDescription;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI nameTextMesh;
        [SerializeField] TextMeshProUGUI lifetimeTextMesh;
        [SerializeField] UniGlow.MultiLanguageText descriptionText;

        // Private
        RectTransform rectTransform;
        Vector2 originalAnchoredPosition;
        Artist selectedArtist;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rectTransform = artistDescription.transform.GetRequiredComponent<RectTransform>();
            originalAnchoredPosition = rectTransform.anchoredPosition;

            Hide();
        }

        private void OnEnable()
        {
            GameEvents.OnArtistSelectionFinished += Show;
            GameEvents.OnArtistSelectionStarted += Hide;
            GameEvents.OnCategorySelectionStarted += Hide;
        }

        private void OnDisable()
        {
            GameEvents.OnArtistSelectionFinished -= Show;
            GameEvents.OnArtistSelectionStarted -= Hide;
            GameEvents.OnCategorySelectionStarted -= Hide;
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void Show(Artist artist)
        {
            if (artist != null) selectedArtist = artist;

            image.sprite = selectedArtist.Image;
            nameTextMesh.text = selectedArtist.Name;
            lifetimeTextMesh.text = selectedArtist.Lifetime;
            descriptionText.SetTexts(selectedArtist.Description, selectedArtist.DescriptionEnglish, selectedArtist.DescriptionPlatt);
            artistDescription.SetActive(true);

            rectTransform.anchoredPosition = tweenOutOfScreenPosition;
            rectTransform.DOAnchorPos(originalAnchoredPosition, tweenDuration).SetEase(tweeningEase);
        }


        void Hide()
        {
            rectTransform.DOAnchorPos(tweenOutOfScreenPosition, tweenDuration).SetEase(tweeningEase).OnComplete(() =>
            {
                artistDescription.SetActive(false);
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
        #endregion



        #region Coroutines

        #endregion
    }
}