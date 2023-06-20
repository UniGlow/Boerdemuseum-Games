using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace ObjectMemory
{
    public class SolutionPanel : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Image image;
        [SerializeField] GameObject continueButton;
        [SerializeField] float buttonAppearDelay = 3f;
        [SerializeField] TextMeshProUGUI solutionText;
        [SerializeField] UniGlow.MultiLanguageText riddleName;
        [SerializeField] UniGlow.MultiLanguageText riddleText;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            continueButton.SetActive(false);
            ShowSolution(GameManager.Instance.CurrentRiddle);
            if (solutionText != null)
            {
                solutionText.text = GameManager.Instance.SolutionChar.ToString();
                solutionText.color = new Color(solutionText.color.r, solutionText.color.g, solutionText.color.b, 0f);
                solutionText.DOColor(new Color(solutionText.color.r, solutionText.color.g, solutionText.color.b, 1f), 1.5f).SetDelay(1f);
            }
            StartCoroutine(Delay(buttonAppearDelay, () => 
            {
                continueButton.SetActive(true);
            }));
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void ShowSolution(Riddle riddle)
        {
            image.sprite = riddle.Image;
            riddleName?.SetTexts(riddle.Name, riddle.NameEnglish, riddle.NamePlatt);
            riddleText?.SetTexts(riddle.Text, riddle.TextEnglish, riddle.TextPlatt);
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