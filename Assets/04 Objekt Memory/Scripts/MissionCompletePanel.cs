using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectMemory
{
	public class MissionCompletePanel : MonoBehaviour
	{

		#region Variable Declarations
		// Serialized Fields
		[SerializeField] GameObject popupPrefab;
		[SerializeField] GameObject welcomePanel;
		[SerializeField] TutorialController tutorialController;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Start()
        {
			UniGlow.GameEvents.LanguageChanged(UniGlow.LanguageManager.CurrentLanguage);
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

		private void OnDisable()
		{
			SaveSystem.Instance.ResetSave();
		}
		#endregion



		#region Public Functions

		#endregion



		#region Private Functions

		#endregion



		#region Coroutines

		#endregion
	}
}