using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace InteractiveMap
{
    public class POIDescriptionControllerImage : POIDescriptionController
    {

        #region Variable Declarations
        // Serialized Fields
        [Header("Image Description Specific")]
        [SerializeField] Image image;
        [SerializeField] POIGallery poiGallery;
        [SerializeField] UniGlow.MultiLanguageText technique;
        [SerializeField] UniGlow.MultiLanguageText dimensions;
        [SerializeField] float imageZoomFactor = 1.75f;
        [SerializeField] Image zoomedImageBackground;

        [Header("Copyright)")]
        [SerializeField] TextMeshProUGUI copyright;
        [Tooltip("Maximal x offset of the copyright text when the sprite ratio is 0,75.")]
        [SerializeField] float xOffsetMax = -60;
        [Tooltip("Maximal y offset of the copyright text when the sprite ratio is 1,5.")]
        [SerializeField] float yOffsetMax = 200;

        // Private
        float copyrightOriginalWidth;
        List<POIGallery> poiGalleryList = new List<POIGallery>();
        bool zoomedOut = false;
        Vector3 imageOriginalPosition;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            copyrightOriginalWidth = copyright.rectTransform.sizeDelta.x;

            imageOriginalPosition = image.rectTransform.localPosition;
        }
        #endregion



        #region Public Functions
        public override void Show(List<POI> pPoiList)
        {
            // Only act when an image needs to be displayed
            if (pPoiList[0].GetData().isAudio) return;

            base.Show(pPoiList);
            ReadData(poiList, 0);
        }

        public override void ReadData(List<POI> pPoiList, int pListPos)
        {
            base.ReadData(pPoiList, pListPos);
            DataSet data = pPoiList[pListPos].GetData();

            image.sprite = data.Image;

            poiGalleryList.Clear();
            foreach (Transform child in scrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }

            int j = pPoiList.Count < 5 ? pPoiList.Count : 5;
            for (int i = 0; i < j; i++)
            {
                POIGallery lPOIGallery = Instantiate(poiGallery);
                lPOIGallery.Initialize(this, scrollViewContent.transform, i);
                poiGalleryList.Add(lPOIGallery);
            }

            for (int i = 0; i < poiGalleryList.Count; i++)
            {
                poiGalleryList[i].SetImage(pPoiList[(pListPos + i) % pPoiList.Count].GetData().Image, pPoiList[(pListPos + i) % pPoiList.Count].GetData().Copyright);
            }
            poiGalleryList[0].Highlight(true);

            technique.SetTexts(data.Technique, data.TechniqueEnglish, data.TechniquePlatt);
            dimensions.SetTexts(data.Dimensions, data.Dimensions, data.Dimensions);
            copyright.text = "© " + data.Copyright;

            //adjust copyright position
            if (data.Image != null)
            {
                float spriteRatio = data.Image.bounds.size.y / data.Image.bounds.size.x; //-> <1 = breiter, >1 = höher
                float percentage = 0.63f / spriteRatio; //-> >1 = muss hoch, <1 muss links
                if (percentage > 1)
                {
                    float yPosition = 285 * (1 - (1 / percentage));
                    yPosition = Mathf.Clamp(yPosition, 0f, yOffsetMax);
                    copyright.rectTransform.anchoredPosition = new Vector2(0f, yPosition);
                    copyright.rectTransform.sizeDelta = new Vector2(copyrightOriginalWidth, copyright.rectTransform.sizeDelta.y);
                }
                else
                {
                    float xPosition = -450 * (1 - percentage);
                    xPosition = Mathf.Clamp(xPosition, xOffsetMax, 0f);
                    copyright.rectTransform.anchoredPosition = new Vector2(xPosition, 0f);
                    copyright.rectTransform.sizeDelta = new Vector2(copyrightOriginalWidth * Mathf.Clamp(percentage, 0.57f, 1f), copyright.rectTransform.sizeDelta.y);
                }
            }
        }

        public void Zoom()
        {
            if (zoomedOut)
            {
                image.transform.DOScale(Vector3.one, tweenDuration).SetEase(tweenEase);
                image.transform.DOLocalMove(imageOriginalPosition, tweenDuration).SetEase(tweenEase);
                zoomedImageBackground.gameObject.SetActive(false);
                zoomedOut = false;
            }
            else
            {
                image.transform.DOScale(new Vector3(imageZoomFactor, imageZoomFactor, 1), tweenDuration).SetEase(tweenEase);
                image.transform.DOLocalMove(Vector3.zero, tweenDuration).SetEase(tweenEase);
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
}