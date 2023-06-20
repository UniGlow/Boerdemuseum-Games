using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wimmelbild
{
    public class LevelController : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Dialogue startDialogue;
        [Tooltip("Time in seconds after which the application will go back to the level selection if no input is received.")]
        [SerializeField] float restartDelay = 180f;

        [Space]
        [SerializeField] bool listenForBackButtons = true;

        // Private
        float timeSinceLastInput;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Start()
        {
            DialogueController.Instance.StartDialogue(startDialogue);
        }

        private void Update()
        {
            timeSinceLastInput += Time.deltaTime;

            if (listenForBackButtons && (Input.GetButtonDown(Constants.INPUT_QUIT) || Input.GetButtonDown(Constants.INPUT_CANCEL))) LoadLevelSelection();

            // Reset level when idle for too long
            if (Input.touchCount > 0 || Input.anyKeyDown) timeSinceLastInput = 0f;
            if (timeSinceLastInput >= restartDelay) LoadLevelSelection();
        }
        #endregion



        #region Public Functions
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadLevelSelection()
        {
            SceneManager.LoadScene("Levelauswahl");
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}