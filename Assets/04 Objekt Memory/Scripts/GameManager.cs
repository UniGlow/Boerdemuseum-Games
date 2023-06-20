using System.Collections.Generic;
using UnityEngine;

namespace ObjectMemory
{
    public class GameManager : MonoBehaviour
    {
        [System.Serializable]
        class Solution
        {
            public enum Language { Deutsch, Englisch, Platt };
            public Language language = Language.Deutsch;
            public string solution;
            public List<Riddle> Riddle = new List<Riddle>();
        }

        #region Variable Declarations
        public static GameManager Instance;

        // Serialized Fields
        [SerializeField] List<Riddle> riddles = new List<Riddle>();
        [SerializeField] string solutionBlanks;
        [SerializeField] List<Solution> solutions = new List<Solution>();

        [Header("References")]
        [SerializeField] GameObject missionCompletePanel;

        // Private
        int solutionNo = -1;
        Riddle currentRiddle;
        int currentRiddleIndex;
        List<Riddle> solvedRiddles = new List<Riddle>();
        List<Riddle> unsolvedRiddles = new List<Riddle>();
        char solutionChar;
        string currentLanguage = Constants.LANGUAGE_GERMAN;
        #endregion



        #region Public Properties
        public Riddle CurrentRiddle { get { return currentRiddle; } }
        public int CurrentRiddleIndex { get { return currentRiddleIndex; } }
        public List<Riddle> Riddles { get { return riddles; } }
        public List<Riddle> UnsolvedRiddles { get { return unsolvedRiddles; } }
        public List<Riddle> SolvedRiddles { get { return solvedRiddles; } }
        public char SolutionChar { get { return solutionChar; } }
        public string SolutionBlanks { get { return solutionBlanks; } }
        public int SolutionNo { get => solutionNo; }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private void OnEnable()
        {
            UniGlow.GameEvents.OnLanguageChanged += Initialize;
        }

        private void OnDisable()
        {
            UniGlow.GameEvents.OnLanguageChanged -= Initialize;
        }

        private void Start()
        {
            
        }
        #endregion



        #region Public Functions
        public void Initialize(string language)
        {
            unsolvedRiddles = new List<Riddle>();
            currentRiddleIndex = 0;

            solutionNo = SaveSystem.Instance.GetSolutionNo();
            
            SetFixedLanguage(language);

            for (int i = 0; i < riddles.Count; i++)
            {
                if (SaveSystem.Instance.GetRiddleState(i))
                {
                    solvedRiddles.Add(riddles[i]);

                    int solutionCharIndex = solutions[solutionNo].Riddle.IndexOf(riddles[i]);
                    solutionBlanks = solutionBlanks.Remove(solutionCharIndex * 2, 1);
                    solutionBlanks = solutionBlanks.Insert(solutionCharIndex * 2, solutions[solutionNo].solution[solutionCharIndex].ToString());
                }
                else unsolvedRiddles.Add(riddles[i]);
            }

            if (unsolvedRiddles.Count > 0) currentRiddle = unsolvedRiddles[currentRiddleIndex];
            else missionCompletePanel.SetActive(true);
        }

        public void SetFixedSolution()
        {
            if (solutionNo < 0) solutionNo = ChooseRandomSolution();
            else if (solutionNo >= 0 && solutionNo <= 3 && currentLanguage == Constants.LANGUAGE_ENGLISH) solutionNo = ChooseRandomSolution();
            else if (solutionNo >= 4 && solutionNo <= 6 && currentLanguage == Constants.LANGUAGE_GERMAN) solutionNo = ChooseRandomSolution();
            Debug.Log(solutions[solutionNo].solution);
        }

        public bool IsCorrectSolution(string qrCodeText)
        {
            if (currentRiddle.QRCode == qrCodeText)
            {
                for (int i = 0; i < solutions[solutionNo].Riddle.Count; i++)
                {
                    if (solutions[solutionNo].Riddle[i] == currentRiddle)
                    {
                        solutionChar = solutions[solutionNo].solution[i];
                        if (i == 0)
                        {
                            solutionBlanks = solutionChar + solutionBlanks.Substring(1);
                        }
                        else if (i == solutions[solutionNo].Riddle.Count - 1)
                        {
                            solutionBlanks = solutionBlanks.Substring(0, i * 2) + solutionChar;
                        }
                        else
                        {
                            solutionBlanks = solutionBlanks.Substring(0, i * 2) + solutionChar + solutionBlanks.Substring(i * 2 + 1);
                        }

                        solvedRiddles.Add(currentRiddle);
                        unsolvedRiddles.Remove(currentRiddle);

                        return true;
                    }
                }
            }

            return false;
        }

        public void NextRiddle(bool pSolved = false)
        {
            if (currentRiddleIndex >= unsolvedRiddles.Count) currentRiddleIndex--;

            if (pSolved) { currentRiddle = unsolvedRiddles[currentRiddleIndex]; return; }

            if (currentRiddleIndex < unsolvedRiddles.Count - 1) currentRiddle = unsolvedRiddles[++currentRiddleIndex];
        }

        public void PrevRiddle()
        {
            if (currentRiddleIndex > 0)
                currentRiddle = unsolvedRiddles[--currentRiddleIndex];
        }
        #endregion



        #region Private Functions
        void SetFixedLanguage(string language)
        {
            if (language == Constants.LANGUAGE_GERMAN)
            {
                currentLanguage = Constants.LANGUAGE_GERMAN;
            }
            else if (language == Constants.LANGUAGE_ENGLISH)
            {
                currentLanguage = Constants.LANGUAGE_ENGLISH;
            }
            else if (language == Constants.LANGUAGE_PLATT)
            {
                currentLanguage = Constants.LANGUAGE_PLATT;
            }

            SetFixedSolution();
        }

        int ChooseRandomSolution()
        {
            Solution.Language lLanguage = Solution.Language.Deutsch;

            if (currentLanguage == Constants.LANGUAGE_ENGLISH)
            {
                lLanguage = Solution.Language.Englisch;
            }
            else if (currentLanguage == Constants.LANGUAGE_PLATT)
            {
                lLanguage = Solution.Language.Platt;
            }

            List<int> lSolutionNOs = new List<int>();

            for (int i = 0; i < solutions.Count; i++)
            {
                if (solutions[i].language == lLanguage) lSolutionNOs.Add(i);
            }

            return lSolutionNOs[Random.Range(0, lSolutionNOs.Count)];
        }
        #endregion



        #region Coroutines

        #endregion
    }
}