using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour 
{

	#region Variable Declarations
	// Serialized Fields
	
	// Private
	
	#endregion
	
	
	
	#region Public Properties
	
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () 
	{
		
	}
	#endregion
	
	
	
	#region Public Functions
	public void StartLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
	#endregion
	
	
	
	#region Private Functions
	
	#endregion
	
	
	
	#region Coroutines
	
	#endregion
}