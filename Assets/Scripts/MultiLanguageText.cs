using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UniGlow
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MultiLanguageText : MonoBehaviour
    {
        [System.Serializable]
        public class LanguageTexts
        {
            [TextArea(1,5)]
            public string GermanText;
            [TextArea(1, 5)]
            public string EnglishText;
            [TextArea(1, 5)]
            public string PlattText;
        }

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] LanguageTexts languageTexts;

        // Private
        TextMeshProUGUI textMesh;
        TMP_InputField inputField;
        #endregion



        #region Public Properties
        public TextMeshProUGUI TextMesh { get => textMesh; }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            textMesh = transform.GetRequiredComponent<TextMeshProUGUI>();
            inputField = transform.GetComponentInParent<TMP_InputField>();
        }

        private void OnEnable()
        {
            GameEvents.OnLanguageChanged += UpdateText;
            UpdateText();
        }

        private void OnDisable()
        {
            GameEvents.OnLanguageChanged -= UpdateText;
        }
        #endregion



        #region Public Functions
        public void SetTexts(string germanText, string englishText, string plattText)
        {
            languageTexts.GermanText = germanText;
            if (englishText != "") languageTexts.EnglishText = englishText;
            else languageTexts.EnglishText = germanText;
            if (englishText != "") languageTexts.PlattText = plattText;
            else languageTexts.PlattText = germanText;

            UpdateText();
        }
        #endregion



        #region Private Functions
        void UpdateText(string language = null)
        {
            if (textMesh == null) textMesh = transform.GetRequiredComponent<TextMeshProUGUI>();

            if (language == null)
            {
                if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_GERMAN)
                {
                    if (inputField) inputField.text = languageTexts.GermanText;
                    else textMesh.text = languageTexts.GermanText;
                }
                else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_ENGLISH)
                {
                    if (inputField) inputField.text = languageTexts.EnglishText;
                    else textMesh.text = languageTexts.EnglishText;
                }
                else if (LanguageManager.CurrentLanguage == Constants.LANGUAGE_PLATT)
                {
                    if (inputField) inputField.text = languageTexts.PlattText;
                    else textMesh.text = languageTexts.PlattText;
                }
            }
            else
            {
                if (language == Constants.LANGUAGE_GERMAN)
                {
                    if (inputField) inputField.text = languageTexts.GermanText;
                    else textMesh.text = languageTexts.GermanText;
                }
                else if (language == Constants.LANGUAGE_ENGLISH)
                {
                    if (inputField) inputField.text = languageTexts.EnglishText;
                    else textMesh.text = languageTexts.EnglishText;
                }
                else if (language == Constants.LANGUAGE_PLATT)
                {
                    if (languageTexts.PlattText != "")
                    {
                        if (inputField) inputField.text = languageTexts.PlattText;
                        else textMesh.text = languageTexts.PlattText;
                    }
                    else 
                    {
                        if (inputField) inputField.text = languageTexts.GermanText;
                        else textMesh.text = languageTexts.GermanText;
                    }
                }
            }
        }
        #endregion



        #region Coroutines

        #endregion
    }
}