using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Wimmelbild
{
    /// <summary>
    /// This class provides public functions to control the dialogue system of the game and is a Singleton.
    /// </summary>
    public class DialogueController : MonoBehaviour
    {
        [System.Serializable]
        public class References
        {
            public GameObject dialogueBox;

            [Space]
            public Image characterNameImage;
            public Image characterImage;

            [Space]
            public UniGlow.MultiLanguageText messageText;

            [Space]
            public AudioSource audioSourceClick;
            public AudioSource audioSourceVoice;

            [Space]
            public Image complementingImage;

            [Space]
            public GameObject buttonContinue;
            public GameObject buttonHintDialogue;
        }

        #region Variable Declarations
        public static DialogueController Instance;

        [Header("Parameters")]
        [Range(0.1f, 0)]
        [Tooltip("Speed of the typing, defined by the delay (in seconds) between the characters.")]
        [SerializeField] float textSpeed = 0.03f;
        [SerializeField] int maxMessageLength = 160;
        [SerializeField] float complementingImageFadeDuration = 0.5f;
        [Tooltip("The amount of dialogues that get buffered to be able to repeat them.")]
        [SerializeField] int bufferedDialogues = 5;
        [Tooltip("The number of dialogues scheduled for repetition by default. Can be changed via code at runtime.")]
        [SerializeField] int numberOfDialoguesOnRepeat = 1;

        [Space]
        [SerializeField] References references;

        Dialogue currentDialogue;
        Message currentMessage;
        int currentMessageInt;
        bool currentMessageFinished;
        bool dialogueInProgress;
        bool dialoguePaused;
        System.Action actionOnEndOfDialogue;
        List<Dialogue> lastDialoguesPlayed = new List<Dialogue>();
        int dialogueToRepeat = -1;
        Dialogue hintDialogue;
        #endregion



        #region Public Properties
        public int NumberOfDialoguesOnRepeat
        {
            get
            {
                if (lastDialoguesPlayed.Count == 0) return 0;
                else return numberOfDialoguesOnRepeat;
            }
            set => numberOfDialoguesOnRepeat = value;
        }
        public bool DialogueInProgress { get { return dialogueInProgress; } }
        #endregion



        #region Unity Event Functions
        void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);

            if (references.characterImage) references.characterImage.enabled = false;
            if (references.characterNameImage) references.characterNameImage.enabled = false;
            references.dialogueBox.SetActive(false);
            references.buttonContinue.SetActive(false);
        }
        #endregion



        #region Custom Event Functions

        #endregion



        #region Public Functions
        /// <summary>
        /// Starts the specified dialogue.
        /// </summary>
        /// <param name="keepLastMessage">Whether the last message of the dialogue should stay visible until it is hidden manually via Stop() or starting a new dialogue.</param>
        /// <param name="onComplete">Method that will be called when the dialogue is completed.</param>
        /// <returns>True if dialogue started successfully, false otherwise.</returns>
        public bool StartDialogue(Dialogue dialogue, Dialogue hintDialogue = null, System.Action onComplete = null)
        {
            // Exception handling
            if (dialogue == null)
            {
                Debug.LogError("Dialogue passed == null. Aborting StartDialogue().", gameObject);
                return false;
            }
            else if (dialogueInProgress)
            {
                Debug.LogWarning("There's already a dialogue active named \"" + currentDialogue.name + "\". \n" + "Aborting the start of dialogue named \"" + dialogue.name + "\".");
                return false;
            }
            else if (dialogue.Messages.Length == 0)
            {
                Debug.LogWarning("The dialogue \"" + dialogue.name + "\" that you are trying to start has no messages.");
            }

            // Set currentDialogue
            currentDialogue = dialogue;
            this.hintDialogue = hintDialogue;
            currentMessageInt = -1;
            dialogueInProgress = true;

            // Fade in the complementing image, if one is defined
            if (references.complementingImage && dialogue.ComplementingImage)
            {
                references.complementingImage.sprite = dialogue.ComplementingImage;
                references.complementingImage.color = new Color(references.complementingImage.color.r, references.complementingImage.color.g, references.complementingImage.color.b, 0f);
                references.complementingImage.DOFade(1f, complementingImageFadeDuration);
                references.complementingImage.enabled = true;
            }
            else if (references.complementingImage) references.complementingImage.enabled = false;

            // Remember the Action to trigger on completion of the dialogue
            if (onComplete != null) actionOnEndOfDialogue = onComplete;

            // Show the UI and start the action
            references.dialogueBox.SetActive(true);
            StartCoroutine(ShowButtonsDelayed(1));
            NextMessage();

            return true;
        }

        /// <summary>
        /// Starts the specified dialogue without the character image and character name.
        /// Internally calls the StartDialogue method.
        /// </summary>
        /// <param name="keepLastMessage">Whether the last message of the dialogue should stay visible until it is hidden manually via Stop() or starting a new dialogue.</param>
        /// <param name="onComplete">Method that will be called when the dialogue is completed.</param>
        /// <returns>True if dialogue started successfully, false otherwise.</returns>
        public bool StartDialogueSimple(Dialogue dialogue, Dialogue hintDialogue = null, System.Action onComplete = null)
        {
            if (references.characterImage) references.characterImage.enabled = false;
            if (references.characterNameImage) references.characterNameImage.enabled = false;

            bool returnValue = StartDialogue(dialogue, hintDialogue, () =>
            {
                if (references.characterImage) references.characterImage.enabled = true;
                if (references.characterNameImage) references.characterNameImage.enabled = true;
                if (onComplete != null) onComplete.Invoke();
            });

            return returnValue;
        }

        /// <summary>
        /// Handles the input to proceed the dialogue.
        /// </summary>
        public void ProceedDialogue()
        {
            if (currentDialogue == null) return;

            if (currentMessageFinished)
            {
                if (currentMessageInt < currentDialogue.Messages.Length - 1) references.audioSourceClick.Play();

                NextMessage();
            }
            else if (!currentMessageFinished)
            {
                FastFinishMessage();
            }

        }

        public void PreviousMessage()
        {
            if (currentDialogue == null || currentMessageInt <= 0) return;

            if (currentMessageFinished)
            {
                references.audioSourceClick.Play();
                FastFinishMessage();
                currentMessageInt -= 2;
                NextMessage();
            }
            else
            {
                FastFinishMessage();
            }
        }

        /// <summary>
        /// Pauses the current dialogue, if one is currently in progress.
        /// </summary>
        public void Pause()
        {
            if (dialogueInProgress)
            {
                references.dialogueBox.SetActive(false);
                dialoguePaused = true;
            }
        }

        /// <summary>
        /// Continues the current dialogue, if one is currently in progress and paused.
        /// </summary>
        public void Continue()
        {
            if (dialoguePaused)
            {
                references.dialogueBox.SetActive(true);
                dialoguePaused = false;
            }
        }

        public void SetMessageFinished(bool value)
        {
            currentMessageFinished = value;
        }

        /// <summary>
        /// Repeats the last x dialogues played.
        /// </summary>
        public void RepeatLastDialogues()
        {
            if (lastDialoguesPlayed.Count == 0)
            {
                Debug.LogError("No dialogue played so far. Can't repeat it.");
                return;
            }
            if (numberOfDialoguesOnRepeat == 0)
            {
                Debug.LogError("No dialogues set to repeat. Aborting repeat.");
                return;
            }

            dialogueToRepeat = lastDialoguesPlayed.Count - numberOfDialoguesOnRepeat;
            StartDialogue(lastDialoguesPlayed[dialogueToRepeat]);
        }

        public void StartHintDialogue()
        {
            Dialogue hintDialogue = this.hintDialogue;
            EndDialogue();
            StartCoroutine(StartDialogueDelayed(1, hintDialogue));
        }
        #endregion



        #region Private Functions
        /// <summary>
        /// Sets the GUI to display the next message of the current dialogue.
        /// </summary>
        void NextMessage()
        {
            references.buttonHintDialogue.SetActive(false);
            currentMessageInt++;
            if (currentMessageInt >= currentDialogue.Messages.Length)
            {
                EndDialogue();
                return;
            }

            currentMessageFinished = false;
            currentMessage = currentDialogue.Messages[currentMessageInt];

            if (currentMessage.CharacterImage && references.characterImage)
            {
                references.characterImage.sprite = currentMessage.CharacterImage;
                references.characterImage.enabled = true;
            }
            else references.characterImage.enabled = false;

            if (currentMessage.CharacterName && references.characterNameImage)
            {
                references.characterNameImage.sprite = currentMessage.CharacterName;
                references.characterNameImage.enabled = true;
            }
            else references.characterNameImage.enabled = false;

            if (currentMessage.Text.Length > maxMessageLength)
            {
                Debug.LogWarning("Message " + currentMessageInt + " of the " +
                currentDialogue.name + " Dialogue is " + currentMessage.Text.Length + " characters long and therefore " +
                "longer than the defined maximal message length of " + maxMessageLength + ".");
            }

            StartCoroutine(TypeText());

            StartCoroutine(PlayAudioOffset());
        }

        /// <summary>
        /// Displays the current message completely immediately.
        /// </summary>
        void FastFinishMessage()
        {
            StopAllCoroutines();
            references.messageText.SetTexts(currentMessage.Text, currentMessage.TextEnglish, currentMessage.TextPlatt);
            if (hintDialogue) references.buttonHintDialogue.SetActive(true);
            references.audioSourceClick.Play();
            currentMessageFinished = true;
        }

        /// <summary>
        /// Ends the current dialogue and resets all variables and UI elements.
        /// </summary>
        void EndDialogue()
        {
            StopAllCoroutines();

            references.audioSourceVoice.Stop();

            // Buffer dialogue to be able to repeat it later
            if (!lastDialoguesPlayed.Contains(currentDialogue)) lastDialoguesPlayed.Add(currentDialogue);
            if (lastDialoguesPlayed.Count > bufferedDialogues) lastDialoguesPlayed.RemoveAt(0);

            // Reset variables
            currentMessageInt = 0;
            currentMessageFinished = false;
            currentMessage = null;
            currentDialogue = null;
            hintDialogue = null;
            dialogueInProgress = false;

            // Currently in a repeat sequence? => repeat next dialogue in line
            if (dialogueToRepeat > -1)
            {
                dialogueToRepeat++;

                // All requested dialogues repeated? => stop the repeating sequence
                if (dialogueToRepeat >= lastDialoguesPlayed.Count)
                {
                    dialogueToRepeat = -1;
                }
                // Else start the next dialogue in line
                else
                {
                    StartDialogue(lastDialoguesPlayed[dialogueToRepeat]);
                    return;
                }
            }

            // Reset UI elements
            references.buttonHintDialogue.SetActive(false);
            references.buttonContinue.SetActive(false);

            references.dialogueBox.SetActive(false);
            if (references.characterNameImage)
            {
                references.characterNameImage.sprite = null;
                references.characterNameImage.enabled = false;
            }
            if (references.characterImage)
            {
                references.characterImage.sprite = null;
                references.characterImage.enabled = false;
            }
            references.messageText.TextMesh.text = "";

            // Trigger the endOfDialogue method if one was defined
            // Make sure to set actionOnEndOfDialogue to null before the call so another dialogue can be triggered.
            if (actionOnEndOfDialogue != null)
            {
                System.Action tempAction = actionOnEndOfDialogue;
                actionOnEndOfDialogue = null;
                tempAction.Invoke();
            }
        }
        #endregion



        #region Coroutines
        /// <summary>
        /// Typewriter effect for text. It's speed can be set through textSpeed in the inspector.
        /// </summary>
        /// <returns></returns>
        IEnumerator TypeText()
        {
            string printString = "";
            if (UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_GERMAN) printString = currentMessage.Text;
            else if (UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_ENGLISH && currentMessage.TextEnglish != "") printString = currentMessage.TextEnglish;
            else if (UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_PLATT && currentMessage.TextPlatt != "") printString = currentMessage.TextPlatt;

            int messageLength = printString.Length;
            printString += "</color>";

            for (int i = 0; i < messageLength + 1; i++)
            {
                printString = printString.Replace("<color=#00000000>", "");
                printString = printString.Insert(i, "<color=#00000000>");

                references.messageText.TextMesh.text = printString;

                yield return new WaitForSeconds(textSpeed);
            }

            references.messageText.TextMesh.text = references.messageText.TextMesh.text.Remove(references.messageText.TextMesh.text.Length - 25);
            //references.messageText.text = references.messageText.text.Replace("<color=#00000000>", "");
            //references.messageText.text = references.messageText.text.Replace("</color>", "");
            if (hintDialogue) references.buttonHintDialogue.SetActive(true);
            currentMessageFinished = true;
        }

        IEnumerator PlayAudioOffset()
        {
            yield return new WaitForSeconds(currentMessage.SoundOffset);

            if (currentMessage.Sound != null && UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_GERMAN) references.audioSourceVoice.PlayOneShot(currentMessage.Sound);
            else if (currentMessage.SoundEnglish != null && UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_ENGLISH) references.audioSourceVoice.PlayOneShot(currentMessage.SoundEnglish);
            else if (currentMessage.SoundPlatt != null && UniGlow.LanguageManager.CurrentLanguage == Constants.LANGUAGE_PLATT) references.audioSourceVoice.PlayOneShot(currentMessage.SoundPlatt);
            //    references.audioSourceTextProceed.PlayOneShot(currentMessage.sound, currentMessage.soundVolume);
        }

        IEnumerator ShowButtonsDelayed(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }
            references.buttonContinue.SetActive(true);
        }

        IEnumerator StartDialogueDelayed(int frames, Dialogue hintDialogue) 
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }
            StartDialogue(hintDialogue);
        }
        #endregion
    }
}