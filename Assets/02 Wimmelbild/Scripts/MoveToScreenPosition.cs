using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToScreenPosition : MonoBehaviour 
{
    public enum Anchor
    {
        MiddleRight
    }

    #region Variable Declarations
    // Serialized Fields
    [SerializeField] Anchor anchor;
    [SerializeField] Vector2 targetPosition;
    [SerializeField] float tweenDuration = 1.5f;
    [SerializeField] Ease tweeningEase = Ease.InOutQuad;

    // Private

    #endregion



    #region Public Properties
    public float TweenDuration { get => tweenDuration; }
    #endregion



    #region Unity Event Functions
    private void OnEnable()
    {
        Move();
    }
    #endregion



    #region Public Functions
    public void Move()
    {
        Vector3 targetPositionWorldSpace = Vector3.zero; 
        if (anchor == Anchor.MiddleRight) targetPositionWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + targetPosition.x, (Screen.height / 2) + targetPosition.y, 10));

        transform.DOMove(targetPositionWorldSpace, tweenDuration).SetEase(tweeningEase);
    }
	#endregion
	
	
	
	#region Private Functions
	
	#endregion
	
	
	
	#region Coroutines
	
	#endregion
}