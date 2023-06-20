using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace UniGlow
{
    public class LanguageSelection : MonoBehaviour, IPointerClickHandler
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Sprite droppedDownSprite;

        // Private
        TMP_Dropdown dropdown;
        int children;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            dropdown = transform.GetRequiredComponent<TMP_Dropdown>();
            children = transform.childCount + 1;
        }

        private void Start()
        {
            if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_GERMAN)
            {
                dropdown.value = 0;
            }
            else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_ENGLISH)
            {
                dropdown.value = 1;
            }
            else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_PLATT)
            {
                dropdown.value = 2;
            }
        }

        private void Update()
        {
            if (transform.childCount < children) ResetCaptionImage();
        }
        #endregion



        #region Public Functions
        public void ChangeLanguage(int languageID)
        {
            if (languageID == 0)
            {
                GameEvents.LanguageChanged(Constants.LANGUAGE_GERMAN);
            }
            else if (languageID == 1)
            {
                GameEvents.LanguageChanged(Constants.LANGUAGE_ENGLISH);
            }
            else if (languageID == 2)
            {
                GameEvents.LanguageChanged(Constants.LANGUAGE_PLATT);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            dropdown.captionImage.sprite = droppedDownSprite;
        }

        public void ResetCaptionImage()
        {
            if (dropdown.captionImage.sprite == droppedDownSprite) dropdown.captionImage.sprite = dropdown.options[dropdown.value].image;
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}