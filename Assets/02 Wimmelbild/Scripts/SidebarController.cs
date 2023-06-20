using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Wimmelbild
{
    [RequireComponent(typeof(AudioSource))]
    public class SidebarController : MonoBehaviour
    {
        #region Variable Declarations
        //Serialized Fields
        [SerializeField] List<Image> searchablesUI = new List<Image>();

        [Header("Searchable Found Events")]
        [SerializeField] List<Dialogue> searchableFoundDialogues = new List<Dialogue>();

        [Space]
        [SerializeField] Dialogue allSearchablesFoundDialogue;
        [SerializeField] AudioClip allSearchablesFoundAudio;
        [SerializeField] UnityEvent onAllSearchablesFound;

        //Private
        int searchablesToFind;
        int searchablesFound = 0;
        AudioSource audioSource;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        void Awake()
        {
            audioSource = transform.GetRequiredComponent<AudioSource>();

            foreach (Image searchable in searchablesUI)
            {
                searchable.color = Color.black;
            }
            searchablesToFind = searchablesUI.Count;
        }

        private void OnEnable()
        {
            GameEvents.OnSearchableFound += HandleSearchableFound;
        }

        private void OnDisable()
        {
            GameEvents.OnSearchableFound -= HandleSearchableFound;
        }
        #endregion



        #region Public Functions
        public void DisplaySearchable(int id)
        {
            searchablesUI[id - 1].color = Color.white;
        }
        #endregion



        #region Private Functions
        private void HandleSearchableFound(Searchable searchable)
        {
            DisplaySearchable(searchable.ID);

            searchablesFound++;

            if (searchablesFound >= searchablesToFind)
            {
                Debug.Log("Alle Searchables gefunden!");
                audioSource.PlayOneShot(allSearchablesFoundAudio, 1f);
                DialogueController.Instance.StartDialogue(allSearchablesFoundDialogue, null, () => { onAllSearchablesFound.Invoke(); });
            }
            else if (searchableFoundDialogues.Count > 0)
            {
                if (searchable.OverrideFoundCongrats != null) DialogueController.Instance.StartDialogue(searchable.OverrideFoundCongrats);
                else DialogueController.Instance.StartDialogue(searchableFoundDialogues[Random.Range(0, searchableFoundDialogues.Count)]);
            }
        }
        #endregion



        #region Coroutines

        #endregion
    }
}
