using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ObjectMemory
{
    public class RiddlePanel : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Image image;
        [SerializeField] UniGlow.MultiLanguageText roomMesh;
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject prevButton;
        [SerializeField] GameObject nextButton;
        [SerializeField] GameObject popupPrefab;
        [SerializeField] GameObject welcomePanel;
        [SerializeField] TutorialController tutorialController;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            if (GameManager.Instance.UnsolvedRiddles.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            ShowRiddle(GameManager.Instance.CurrentRiddle);
            RefreshButtons();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PopupMessage popup = Instantiate(popupPrefab, transform).transform.GetRequiredComponent<PopupMessage>();
                popup.Initialize("", () => 
                {
                    welcomePanel.SetActive(true);
                    gameObject.SetActive(false);
                    tutorialController.ResetTutorials();
                });
            }
        }
        #endregion



        #region Public Functions
        public void RefreshRiddle()
        {
            ShowRiddle(GameManager.Instance.CurrentRiddle);
        }

        public void RefreshButtons()
        {
            if (GameManager.Instance.CurrentRiddleIndex == 0) prevButton.SetActive(false);
            else prevButton.SetActive(true);

            if (GameManager.Instance.CurrentRiddleIndex >= GameManager.Instance.UnsolvedRiddles.Count - 1) nextButton.SetActive(false);
            else nextButton.SetActive(true);
        }
        #endregion



        #region Private Functions
        void ShowRiddle(Riddle riddle)
        {
            image.sprite = riddle.Image;
            image.GetComponent<RectTransform>().localPosition = riddle.ZoomLocalPosition;
            if (riddle.ZoomScale != 0) image.GetComponent<RectTransform>().localScale = riddle.ZoomScale * Vector3.one;
            roomMesh.SetTexts(riddle.Room, riddle.RoomEnglish, riddle.Room);
            if (audioSource && riddle.Voiceover) audioSource.PlayOneShot(riddle.Voiceover);
        }
        #endregion



        #region Coroutines

        #endregion
    }
}