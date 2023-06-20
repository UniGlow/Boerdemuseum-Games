using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Kuenstlerportraits
{
    public class CategorySelection : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Space]
        [SerializeField] Artist artistOnStart;

        [Header("Animations")]
        [SerializeField] Vector2 foldedInSizeDelta;
        [SerializeField] float tweenDuration = 1f;
        [SerializeField] Ease tweeningEase = Ease.InOutCubic;

        [Header("References")]
        [SerializeField] List<GameObject> categoryButtons = new List<GameObject>();

        // Private
        RectTransform rectTransform;
        Vector2 foldedOutSizeDelta;
        Artist currentlySelectedArtist;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            rectTransform = transform.GetRequiredComponent<RectTransform>();
            foldedOutSizeDelta = rectTransform.sizeDelta;
        }

        private void Start()
        {
            ShowCategories(artistOnStart);
        }

        private void OnEnable()
        {
            GameEvents.OnArtistSelectionFinished += ShowCategories;
            GameEvents.OnArtistSelectionStarted += HideCategories;
        }

        private void OnDisable()
        {
            GameEvents.OnArtistSelectionFinished -= ShowCategories;
            GameEvents.OnArtistSelectionStarted -= HideCategories;
        }
        #endregion



        #region Public Functions
        public void ShowCategory(int buttonNumber)
        {
            ActivateAllButtons(true);

            categoryButtons[buttonNumber].GetComponentInChildren<Button>().enabled = false;

            categoryButtons[buttonNumber].GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 1.0f);

            categoryButtons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            categoryButtons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.8f, 0.8f, 0.8f, 1.0f);

            GameEvents.CategorySelectionStarted(currentlySelectedArtist.Categories[buttonNumber]);

            StartCoroutine(Wait(tweenDuration, () => 
            { 
                GameEvents.CategorySelectionFinished(currentlySelectedArtist.Categories[buttonNumber]); 
            }));
        }
        #endregion



        #region Private Functions
        void ShowCategories(Artist artist)
        {
            if (artist == null) return;

            ActivateAllButtons(false);

            // Rename categories
            for (int i = 0; i < categoryButtons.Count; i++)
            {
                if (artist.Categories.Count > i)
                {
                    categoryButtons[i].SetActive(true);
                    categoryButtons[i].GetComponentInChildren<UniGlow.MultiLanguageText>().SetTexts(artist.Categories[i].Name, artist.Categories[i].NameEnglish, artist.Categories[i].NamePlatt);
                }
                else
                {
                    categoryButtons[i].SetActive(false);
                }
            }

            currentlySelectedArtist = artist;

            rectTransform.DOSizeDelta(foldedOutSizeDelta, tweenDuration).SetEase(tweeningEase).OnComplete(() =>
            {
                ActivateAllButtons(true);
            }); ;
        }

        void HideCategories(Artist artist)
        {
            if (artist == null)
            {
                ActivateAllButtons(true);
                return;
            }

            ActivateAllButtons(false);

            rectTransform.DOSizeDelta(foldedInSizeDelta, tweenDuration).SetEase(tweeningEase).OnComplete(() => 
            {
                ActivateAllButtons(true);
            });
        }

        void ActivateAllButtons(bool active)
        {
            foreach (Button button in transform.GetComponentsInChildren<Button>())
            {
                button.enabled = active;

                button.gameObject.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 0f);

                button.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
        }
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