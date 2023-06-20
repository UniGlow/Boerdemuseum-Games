using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupMessage : MonoBehaviour 
{
    public enum ButtonPress
    {
        None,
        Ok,
        Cancel
    }

    #region Variable Declarations
    public static PopupMessage Instance;

    // Serialized Fields
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;

    // Private
    ButtonPress buttonPressed;
    #endregion



    #region Public Properties
    public ButtonPress ButtonPressed { get => buttonPressed; }
    #endregion



    #region Unity Event Functions
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(gameObject);
    }
    #endregion



    #region Public Functions
    public void Initialize(string question, Action okAction, Action cancelAction = null)
    {
        if (question != "") this.question.text = question;
        okButton.onClick.AddListener(() => { okAction.Invoke(); });
        if (cancelAction != null) cancelButton.onClick.AddListener(() => { cancelAction.Invoke(); });
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
	#endregion
	
	
	
	#region Private Functions
	
	#endregion
	
	
	
	#region Coroutines
	public IEnumerator Initialize(string question)
    {
        if (question != "") this.question.text = question;
        
        var waitForButton = new WaitForUIButtons(okButton, cancelButton);

        yield return waitForButton.Reset();
        if(waitForButton.PressedButton == okButton)
        {
            buttonPressed = ButtonPress.Ok;
        }
        else
        {
            buttonPressed = ButtonPress.Cancel;
        }
    }
	#endregion
}