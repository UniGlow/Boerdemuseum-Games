using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// This class offers public functions to fade the screen in and out from and to an image.
/// </summary>
[RequireComponent(typeof(Image))]
public class FadeOutScreen : MonoBehaviour
{
    Image fadeOutImage;
    bool fading = false;



    private void Awake()
    {
        fadeOutImage = transform.GetRequiredComponent<Image>();
    }



    /// <summary>
    /// Fades in the screen from the image on this GameObject.
    /// </summary>
    /// <param name="fadeInTime">Time in seconds the Fade-In shall take</param>
    public void FadeIn(float fadeInTime)
    {
        if (fading)
        {
            Debug.LogWarning("FadeIn couldn't start, because a fading is already in process.");
            return;
        }

        fading = true;

        fadeOutImage.DOFade(0f, fadeInTime).SetEase(Ease.InOutQuad).OnComplete(() => 
        {
            fadeOutImage.enabled = false;
            fading = false;
        });
    }

    /// <summary>
    /// Fades in the screen from the image on this GameObject.
    /// </summary>
    /// <param name="fadeInTime">Time in seconds the Fade-In shall take</param>
    /// <param name="onComplete">Function to be called when the FadeIn is completed</param>
    public void FadeIn(float fadeInTime, System.Action onComplete)
    {
        if (fading)
        {
            Debug.LogWarning("FadeIn couldn't start, because a fading is already in process.");
            return;
        }

        fading = true;

        fadeOutImage.DOFade(0f, fadeInTime).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            fadeOutImage.GetComponent<Image>().enabled = false;
            fading = false;
            onComplete.Invoke();
        });
    }

    /// <summary>
    /// Fades the screen to the image on this GameObject.
    /// </summary>
    /// <param name="fadeOutTime">Time in seconds th Fade-Out shall take</param>
    public void FadeOut(float fadeOutTime)
    {
        if (fading)
        {
            Debug.LogWarning("FadeOut couldn't start, because a fading is already in process.");
            return;
        }

        fading = true;

        fadeOutImage.enabled = true;
        fadeOutImage.DOFade(1f, fadeOutTime).SetEase(Ease.InOutQuad).OnComplete(() => 
        {
            fading = false;
        });
    }

    /// <summary>
    /// Fades the screen to the image on this GameObject.
    /// </summary>
    /// <param name="fadeOutTime">Time in seconds th Fade-Out shall take</param>
    /// <param name="onComplete">Function to be called when the FadeOut is completed</param>
    public void FadeOut(float fadeOutTime, System.Action onComplete)
    {
        if (fading)
        {
            Debug.LogWarning("FadeOut couldn't start, because a fading is already in process.");
            return;
        }

        fading = true;

        fadeOutImage.enabled = true;
        fadeOutImage.DOFade(1f, fadeOutTime).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            fading = false;
            onComplete.Invoke();
        });
    }
}
