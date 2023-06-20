using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour 
{

    #region Variable Declarations
    // Serialized Fields
    [Tooltip("The amount of time in seconds that needs to pass until an input is evaluated after a tutorial screen appears.")]
    [SerializeField] float delayForInput = 3f;

    [Space]
    [SerializeField] bool showOnStart = false;
    [ConditionalHide("showOnStart", true, false)]
    [SerializeField] int tutorialPageToShowOnStart = 0;

    [Header("Button Colors")]
    [SerializeField] Color activeButtonColor;
    [SerializeField] Color inactiveButtonColor;

    [Space]
    [SerializeField] Color activeTextColor = Color.white;
    [SerializeField] Color inactiveTextColor = Color.white;

    // Private
    int currentTutorial = -1;
    List<Transform> tutorials = new List<Transform>();
    #endregion



    #region Public Properties

    #endregion



    #region Unity Event Functions
    private void Awake()
    {
        currentTutorial = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            tutorials.Add(transform.GetChild(i));
            if (transform.GetChild(i).GetComponentInChildren<Button>().onClick == null) Debug.LogError("There is no action set for the continueButton of tutorial " + transform.GetChild(i) + ". Please set one in the inspector.", transform.GetChild(i));
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Start ()
    {
        if (showOnStart) ShowTutorial(tutorialPageToShowOnStart);
	}

    private void OnDisable()
    {
        HideTutorial();
    }
    #endregion



    #region Public Functions
    public void ShowTutorial(int childNumber)
    {
        if (childNumber >= tutorials.Count)
        {
            Debug.LogError("Tutorial number " + childNumber + " not defined.", gameObject);
            return;
        }

        tutorials[childNumber].gameObject.SetActive(true);
        Time.timeScale = 0f;
        currentTutorial = childNumber;
        StartCoroutine(BlockInput());
    }

    public void ShowNextTutorial()
    {
        HideTutorial(false);
        ShowTutorial(currentTutorial + 1);
    }

    public void ShowTutorial(string name)
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].name.Contains(name))
            {
                ShowTutorial(i);
            }
        }
    }

    public void HideTutorial(bool resetCurrentTutorial = true)
    {
        foreach (Transform tutorial in tutorials)
        {
            tutorial.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        if (resetCurrentTutorial) currentTutorial = -1;
    }

    public void ResetTutorials()
    {
        currentTutorial = -1;
    }
    #endregion



    #region Private Functions
    void ActivateContinueButton(Button button)
    {
        button.enabled = true;

        button.GetComponent<Image>().color = activeButtonColor;

        TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh) textMesh.color = activeTextColor;
    }

    void DeactivateContinueButton(Button button)
    {
        button.enabled = false;

        button.GetComponent<Image>().color = inactiveButtonColor;

        TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh) textMesh.color = inactiveTextColor;
    }
    #endregion



    #region Coroutines
    IEnumerator BlockInput()
    {
        Button button = transform.GetChild(currentTutorial).GetComponentInChildren<Button>();
        DeactivateContinueButton(button);
        yield return new WaitForSecondsRealtime(delayForInput);
        ActivateContinueButton(button);
    }
    #endregion
}