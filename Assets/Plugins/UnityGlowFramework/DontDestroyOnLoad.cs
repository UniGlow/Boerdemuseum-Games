using UnityEngine;

/// <summary>
/// Flags this GameObject (and all its children) as "DontDestroyOnLoad".
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour 
{
	private void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}
}