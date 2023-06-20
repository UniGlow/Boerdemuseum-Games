using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageScaler : MonoBehaviour
{
    #region Variable Declarations
    // Serialized Fields
    [SerializeField] Image zoomImage;
    [SerializeField] float imageZoomFactor = 1.75f;
    [SerializeField] Image zoomedImageBackground;
    [SerializeField] Vector3 positionToZoomTo;

    [Header("Animations")]
    [SerializeField] protected float tweenDuration = 1f;
    [SerializeField] protected Ease tweenEase = Ease.InOutCubic;

    // Private
    bool zoomedOut = false;
    Vector3 imageOriginalPosition;
    #endregion


    #region Public Properties

    #endregion


    #region Unity Event Functions
    private void Awake()
    {
        imageOriginalPosition = zoomImage.rectTransform.localPosition;
    }
    #endregion


    #region Public Functions
    public void Zoom()
    {
        if (zoomedOut)
        {
            zoomImage.transform.DOScale(Vector3.one, tweenDuration).SetEase(tweenEase);
            zoomImage.transform.DOLocalMove(imageOriginalPosition, tweenDuration).SetEase(tweenEase);
            zoomedImageBackground.gameObject.SetActive(false);
            zoomedOut = false;
        }
        else
        {
            zoomImage.transform.DOScale(new Vector3(imageZoomFactor, imageZoomFactor, 1), tweenDuration).SetEase(tweenEase);
            zoomImage.transform.DOLocalMove(positionToZoomTo, tweenDuration).SetEase(tweenEase);
            zoomedImageBackground.gameObject.SetActive(true);
            zoomedOut = true;
        }
    }
    #endregion


    #region Private Functions

    #endregion


    #region Coroutines

    #endregion
}
