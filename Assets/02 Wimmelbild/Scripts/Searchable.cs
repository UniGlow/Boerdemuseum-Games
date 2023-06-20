using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using cakeslice;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Wimmelbild
{
    [RequireComponent(typeof(AudioSource))]
    public class Searchable : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] int id;

        [SerializeField] cakeslice.Outline outline = null;

        [Header("Hinting")]
        [SerializeField] float initialTimeOffset = 30f;
        [SerializeField] float secondsUntilNextHint = 25f;
        [SerializeField] float secondsBlinkOnDuration = 0.35f;
        [SerializeField] float secondsBlinkOffDuration = 0.25f;
        [SerializeField] int blinkThisManyTimes = 3;

        [Header("Dialogue")]
        [SerializeField] Dialogue foundDialogue;
        [SerializeField] Dialogue foundDialogue2;
        [SerializeField] Dialogue overrideFoundCongrats;

        [Header("Events")]
        [SerializeField] UnityEvent afterClick;
        [SerializeField] UnityEvent afterAnimation;
        [SerializeField] UnityEvent afterSidebar;
        [SerializeField] UnityEvent afterDialogue;
        [SerializeField] UnityEvent afterDialogue2;

        [Header("Catch Animation")]
        [SerializeField] float catchBlinkDuration = 0.2f;
        [SerializeField] int catchBlinkAmount = 2;
        [SerializeField] Ease catchBlinkEase;
        [SerializeField] bool hideAfterCatch = false;

        // Private
        bool isCatched = false;
        float lastHintTimer = 0f;
        AudioSource audioSource;
        bool clickable = true;
        #endregion



        #region Public Properties
        public Sprite Sprite { get { return GetComponent<SpriteRenderer>().sprite; } }
        public int ID { get { return id; } }
        public Dialogue OverrideFoundCongrats { get => overrideFoundCongrats; }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            audioSource = transform.GetRequiredComponent<AudioSource>();
            outline.enabled = false;
            lastHintTimer = initialTimeOffset;
        }

        private void OnEnable()
        {
            GameEvents.OnSearchableKlicked += HandleSearchableKlicked;
            GameEvents.OnSearchableFound += HandleSearchableFound;
        }

        private void OnDisable()
        {
            GameEvents.OnSearchableKlicked -= HandleSearchableKlicked;
            GameEvents.OnSearchableFound -= HandleSearchableFound;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!clickable || isCatched || DialogueController.Instance.DialogueInProgress) return;

            outline.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!clickable || isCatched || DialogueController.Instance.DialogueInProgress) return;

            GameEvents.SearchableKlicked(this);
            CollectSearchable();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isCatched) return;

            outline.enabled = false;
        }

        void Update()
        {
            if (!isCatched && !DialogueController.Instance.DialogueInProgress && Time.time > lastHintTimer + secondsUntilNextHint)
            {
                lastHintTimer = Time.time;
                StartCoroutine(GiveHint());
            }
        }
        #endregion



        #region Public Functions
        public void Animate(GameObject animationPanel)
        {
            animationPanel.SetActive(true);
            StartCoroutine(AfterAnimation(animationPanel, true, () => 
            {
                afterAnimation.Invoke();
            }));
        }

        public void AnimateAndHide(GameObject animationPanel)
        {
            animationPanel.SetActive(true);
            StartCoroutine(AfterAnimation(animationPanel, false, () =>
            {
                afterAnimation.Invoke();
            }));
        }

        public void MoveToSidebar(GameObject moveToSidebarAnimation)
        {
            moveToSidebarAnimation.SetActive(true);
            StartCoroutine(AfterAnimation(moveToSidebarAnimation, false, () => 
            {
                afterSidebar.Invoke();
            }));
        }

        public void StartDialogue()
        {
            if (foundDialogue)
            {
                DialogueController.Instance.StartDialogue(foundDialogue, null, () =>
                {
                    afterDialogue.Invoke();
                });
            }
            else
            {
                afterDialogue.Invoke();
            }
        }

        public void StartDialogue2()
        {
            if (foundDialogue2)
            {
                DialogueController.Instance.StartDialogue(foundDialogue2, null, () =>
                {
                    afterDialogue2.Invoke();
                });
            }
            else
            {
                afterDialogue2.Invoke();
            }
        }

        public void CompleteCatching()
        {
            if (hideAfterCatch) gameObject.SetActive(false);
            GameEvents.SearchableFound(this);
        }
        #endregion



        #region Private Functions
        void CollectSearchable()
        {
            isCatched = true;
            outline.enabled = true;
            audioSource.Play();

            DOTween.To(() => OutlineEffect.Instance.lineColor1, x => OutlineEffect.Instance.lineColor1 = x, 
                new Color(OutlineEffect.Instance.lineColor1.r, OutlineEffect.Instance.lineColor1.g, OutlineEffect.Instance.lineColor1.b, 0f), catchBlinkDuration)
                .SetEase(catchBlinkEase).SetLoops(catchBlinkAmount * 2, LoopType.Yoyo).OnComplete(() => 
                {
                    outline.enabled = false;
                    afterClick.Invoke();
                });
        }

        void DeactivateSearchable()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        void HandleSearchableKlicked(Searchable searchable)
        {
            clickable = false;
        }

        void HandleSearchableFound(Searchable searchable)
        {
            clickable = true;
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

        IEnumerator AfterAnimation(GameObject animationPanel, bool keepActive, System.Action onComplete)
        {
            Animation animation = animationPanel.GetComponentInChildren<Animation>();
            if (animation) yield return new WaitForSeconds(animationPanel.GetComponentInChildren<Animation>().clip.length);
            else yield return new WaitForSeconds(animationPanel.GetComponentInChildren<MoveToScreenPosition>().TweenDuration);

            animationPanel.SetActive(keepActive);
            onComplete.Invoke();
            DeactivateSearchable();
        }
        #endregion
    }
}