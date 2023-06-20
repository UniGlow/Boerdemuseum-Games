using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow
{
    public class ApplicationQuitter : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Update()
        {
            if (Input.GetButtonDown(Constants.INPUT_QUIT))
            {
                Quit();
            }
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
        #endregion



        #region Coroutines

        #endregion
    }
}