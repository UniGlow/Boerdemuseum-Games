using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.EventSystems;

namespace Wimmelbild
{
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(AudioSource))]
    public class Zoomable : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Dialogue dialogue;
        [SerializeField] Dialogue hintDialogue;

        [Space]
        [Tooltip("You can link the searchable this zoomable gives a specific hint for. If linked, this zoomable will only show its hint as an option when the referenced searchable hasn't been found yet.")]
        [SerializeField] Searchable correspondingSearchable;

        [Header("Hinting")]
        [SerializeField] float initialTimeOffset = 30f;
        [SerializeField] float secondsBetweenHints = 30f;
        [SerializeField] float secondsBlinkOnDuration = 0.35f;
        [SerializeField] float secondsBlinkOffDuration = 0.25f;
        [SerializeField] int blinkThisManyTimes = 3;

        // Private
        Outline outline = null;
        AudioSource audioSource;
        float lastHintTimer = 0f;
        bool correspondingSearchableFound;
        bool clickable = true;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Start()
        {
            outline = transform.GetRequiredComponent<Outline>();
            outline.enabled = false;
            audioSource = transform.GetRequiredComponent<AudioSource>();
            lastHintTimer = initialTimeOffset;
        }

        private void OnEnable()
        {
            GameEvents.OnSearchableKlicked += HandleSearchableKlicked;
            GameEvents.OnSearchableFound += RegisterFoundSearchable;
        }

        private void OnDisable()
        {
            GameEvents.OnSearchableFound -= HandleSearchableKlicked;
            GameEvents.OnSearchableFound -= RegisterFoundSearchable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!clickable || DialogueController.Instance.DialogueInProgress) return;

            outline.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!clickable || DialogueController.Instance.DialogueInProgress) return;

            Collect();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            outline.enabled = false;
        }

        void Update()
        {
            if (!DialogueController.Instance.DialogueInProgress && Time.time > lastHintTimer + secondsBetweenHints)
            {
                lastHintTimer = Time.time;
                StartCoroutine(GiveHint());
            }
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void Collect()
        {
            outline.enabled = false;
            audioSource.Play();
            if (!correspondingSearchableFound) DialogueController.Instance.StartDialogue(dialogue, hintDialogue);
            else DialogueController.Instance.StartDialogue(dialogue);
        }

        void RegisterFoundSearchable(Searchable searchable)
        {
            if (searchable == correspondingSearchable) correspondingSearchableFound = true;

            clickable = true;
        }

        void HandleSearchableKlicked(Searchable searchable)
        {
            clickable = false;
        }
        #endregion



        #region Coroutines
        private IEnumerator GiveHint()
        {
            for (int i = 0; i < blinkThisManyTimes; i++)
            {
                if (outline) outline.enabled = true;
                yield return new WaitForSeconds(secondsBlinkOnDuration);
                if (outline) outline.enabled = false;
                yield return new WaitForSeconds(secondsBlinkOffDuration);
            }
        }
        #endregion
    }
}