using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniGlow
{
    [RequireComponent(typeof(Image))]
    public class MultiLanguageImage : MonoBehaviour
    {

        [System.Serializable]
        public class LanguageSprites
        {
            public Sprite GermanSprite;
            public Sprite EnglishSprite;
            public Sprite PlattSprite;
        }

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] LanguageSprites languageSprites;

        // Private
        Image image;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            image = transform.GetRequiredComponent<Image>();
        }

        private void OnEnable()
        {
            GameEvents.OnLanguageChanged += UpdateImage;
            UpdateImage();
        }

        private void OnDisable()
        {
            GameEvents.OnLanguageChanged -= UpdateImage;
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void UpdateImage(string language = null)
        {
            if (image == null) image = transform.GetRequiredComponent<Image>();

            if (language == null)
            {
                if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_GERMAN)
                {
                    image.sprite = languageSprites.GermanSprite;
                }
                else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_ENGLISH)
                {
                    image.sprite = languageSprites.EnglishSprite;
                }
                else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_PLATT)
                {
                    if (languageSprites.PlattSprite != null) image.sprite = languageSprites.PlattSprite;
                    else image.sprite = languageSprites.GermanSprite;
                }
            }
            else
            {
                if (language == Constants.LANGUAGE_GERMAN)
                {
                    image.sprite = languageSprites.GermanSprite;
                }
                else if (language == Constants.LANGUAGE_ENGLISH)
                {
                    image.sprite = languageSprites.EnglishSprite;
                }
                else if (language == Constants.LANGUAGE_PLATT)
                {
                    if (languageSprites.PlattSprite != null) image.sprite = languageSprites.PlattSprite;
                    else image.sprite = languageSprites.GermanSprite;
                }
            }
        }
        #endregion



        #region Coroutines

        #endregion
    }
}