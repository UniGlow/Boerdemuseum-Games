using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ObjectMemory
{
    public class Scanner : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] GameObject solutionPanel;
        [SerializeField] QRCodeDecodeController e_qrController;
        [SerializeField] GameObject wrongScanMessage;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            e_qrController.StartWork();
            wrongScanMessage.SetActive(false);
        }

        private void OnDisable()
        {
            e_qrController.StopWork();
        }
        #endregion



        #region Public Functions
        public void qrScanFinished(string dataText)
        {
            if (GameManager.Instance.IsCorrectSolution(dataText)) SolveRiddle();
            else
            {
                Debug.Log(dataText);
                e_qrController.Reset();
                StopAllCoroutines();
                wrongScanMessage.SetActive(true);
                StartCoroutine(Delay(2, () =>
                {
                    wrongScanMessage.SetActive(false);
                }));
            }
        }
        #endregion



        #region Private Functions
        void SolveRiddle()
        {
            solutionPanel.SetActive(true);
            gameObject.SetActive(false);

            SaveSystem.Instance.SaveRiddleCompletion(GameManager.Instance.Riddles.IndexOf(GameManager.Instance.CurrentRiddle), GameManager.Instance.SolutionNo);
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