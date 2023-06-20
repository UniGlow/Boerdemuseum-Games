using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ObjectMemory
{
    public class OverviewPanel : MonoBehaviour
    {
        #region Variable Declarations
        // Serialized Fields
        [SerializeField] List<Image> riddleThumbnails = new List<Image>();
        [SerializeField] Sprite uncompletedRiddle;
        [SerializeField] GameObject riddlePanel;
        [SerializeField] GameObject missionCompletePanel;
        [SerializeField] TextMeshProUGUI missionCompleteSolution;
        [SerializeField] TextMeshProUGUI solutionText;
        [SerializeField] GameObject continueButton;

        [Space]
        [SerializeField] float animationDelay = 2f;
        [SerializeField] float animationDuration = 2f;
        [SerializeField] Ease animationEase = Ease.InOutCubic;

        [Space]
        [SerializeField] TutorialController tutorialController;

        // Private
        bool tutorialPlayed;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Start()
        {
            for (int i = 0; i < GameManager.Instance.Riddles.Count; i++)
            {
                riddleThumbnails[i].sprite = GameManager.Instance.Riddles[i].Image;
                riddleThumbnails[i].color = Color.black;
            }
        }

        private void OnEnable()
        {
            continueButton.SetActive(false);
            InitializeThumbnails(GameManager.Instance.SolvedRiddles);
            solutionText.text = GameManager.Instance.SolutionBlanks;
            StartCoroutine(Delay(animationDelay, () =>
            {
                AnimateLastCompletedRiddle(GameManager.Instance.CurrentRiddle);
            }));
        }
        #endregion



        #region Public Functions
        public void Continue()
        {
            // Continue with next riddle
            if (GameManager.Instance.UnsolvedRiddles.Count > 0)
            {
                GameManager.Instance.NextRiddle(true);
                riddlePanel.SetActive(true);
            }
            // End mission
            else
            {
                missionCompletePanel.SetActive(true);
                missionCompleteSolution.text = GameManager.Instance.SolutionBlanks;
            }
            gameObject.SetActive(false);
        }
        #endregion



        #region Private Functions
        void InitializeThumbnails(List<Riddle> solvedRiddles)
        {
            List<Riddle> lList = new List<Riddle>(solvedRiddles);
            lList.Remove(GameManager.Instance.CurrentRiddle);

            foreach (Riddle lRiddle in lList)
            {
                riddleThumbnails[GameManager.Instance.Riddles.IndexOf(lRiddle)].color = Color.white;
            }
        }

        void AnimateLastCompletedRiddle(Riddle riddle)
        {
            int index = GameManager.Instance.Riddles.IndexOf(riddle);
            riddleThumbnails[index].DOColor(new Color(riddleThumbnails[index].color.r, riddleThumbnails[index].color.g, riddleThumbnails[index].color.b, 0f), animationDuration)
            .SetEase(animationEase).OnComplete(() =>
            {
                riddleThumbnails[index].sprite = riddle.Image;
                riddleThumbnails[index].DOColor(new Color(1f, 1f, 1f, 1f), animationDuration)
                .SetEase(animationEase).OnComplete(() =>
                {
                    continueButton.SetActive(true);
                    if (!tutorialPlayed)
                    {
                        tutorialController.ShowTutorial("Overview");
                        tutorialPlayed = true;
                    }
                });
            });
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