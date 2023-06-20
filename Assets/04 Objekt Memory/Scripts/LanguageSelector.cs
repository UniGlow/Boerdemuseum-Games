using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectMemory
{
	public class LanguageSelector : MonoBehaviour
	{

		#region Variable Declarations
		// Serialized Fields
		[SerializeField] GameObject popupPrefab;
		[SerializeField] UnityEvent onClick;

		// Private

		#endregion



		#region Public Properties

		#endregion



		#region Unity Event Functions
		private void Start()
		{

		}
		#endregion



		#region Public Functions
		public void SelectLanguage()
        {
			PopupMessage popup = Instantiate(popupPrefab, transform.GetComponentInParent<Canvas>().transform).transform.GetRequiredComponent<PopupMessage>();
			popup.Initialize("", () => { onClick.Invoke(); });
        }
		#endregion



		#region Private Functions

		#endregion



		#region Coroutines

		#endregion
	}
}