﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes the color of an image on the same GameObject over time according to the defined color Gradient.
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageColorChanger : MonoBehaviour
{

    #region Variable Declarations
    [SerializeField] Gradient colors = null;
    [Tooltip("Time it takes to go through all colors of the defined color gradient.")]
    [SerializeField] float fadeTime = 2f;

    float timer;
    Image image;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start()
    {
        image = GetComponent<Image>();
	}
	
	private void Update()
    {
        timer += Time.deltaTime;

        image.color = colors.Evaluate((timer / fadeTime));

        if (timer >= fadeTime) timer = 0;
	}
	#endregion
	
	
	
	#region Private Functions
	#endregion
}
