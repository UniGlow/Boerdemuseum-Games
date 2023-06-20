using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ObjectMemory
{
	[System.Serializable]
	public class SaveData
    {
		public List<bool> riddles = new List<bool>();
		public int solutionNo;
    }

	public class SaveSystem : MonoBehaviour
	{

		#region Variable Declarations
		public static SaveSystem Instance;

		// Serialized Fields
		[Header("Debug")]
		[SerializeField] bool verbose = true;

		// Private
		SaveData saveData;
		#endregion



		#region Public Properties

		#endregion



		#region Unity Event Functions
		private void Awake()
		{
			if (Instance == null) Instance = this;
			else if (Instance != this) Destroy(gameObject);

			transform.parent = null;
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			LoadSaveData();
		}
		#endregion



		#region Public Functions
		public void SaveRiddleCompletion(int number, int solutionNo)
        {
			saveData.riddles[number] = true;
			saveData.solutionNo = solutionNo;

			SaveToFile();
        }

		public bool GetRiddleState(int number)
        {
			return saveData.riddles[number];
        }

		public int GetSolutionNo()
        {
			return saveData.solutionNo;
        }

		public void ResetSave()
        {
            for (int i = 0; i < saveData.riddles.Count; i++)
            {
				saveData.riddles[i] = false;
				saveData.solutionNo = -1;
            }

			string path = Application.persistentDataPath + "/saves.data";
			if (File.Exists(path)) File.Delete(path);

			Debug.Log("Resetted save state.");
        }

		public void LoadSaveData()
		{
			string path = Application.persistentDataPath + "/saves.data";
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				saveData = formatter.Deserialize(stream) as SaveData;

				if (verbose) Debug.Log("Loaded saved data from file.");
			}
			else
			{
				Debug.LogWarning("Save file not found in " + path + ". Creating new save.");

				saveData = new SaveData();
                for (int i = 0; i < GameManager.Instance.Riddles.Count; i++)
                {
					saveData.riddles.Add(false);
                }
				saveData.solutionNo = -1;
			}
		}
		#endregion



		#region Private Functions
		private void SaveToFile()
		{
			BinaryFormatter formatter = new BinaryFormatter();

			string path = Application.persistentDataPath + "/saves.data";
			FileStream stream = new FileStream(path, FileMode.Create);

			formatter.Serialize(stream, saveData);
			stream.Close();

			if (verbose) Debug.Log("Saved game progress to file.");
		}
		#endregion



		#region Coroutines

		#endregion
	}
}