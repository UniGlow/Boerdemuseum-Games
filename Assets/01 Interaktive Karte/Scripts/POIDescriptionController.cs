using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace InteractiveMap
{
    public class POIDescriptionController : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] protected GameObject panel;
        [SerializeField] protected GameObject scrollViewContent;
        [SerializeField] protected GameObject buttonLeft;
        [SerializeField] protected GameObject buttonRight;
        [SerializeField] protected Image background;
        [SerializeField] protected UniGlow.MultiLanguageText description;
        [SerializeField] protected UniGlow.MultiLanguageText author;
        [SerializeField] protected UniGlow.MultiLanguageText date;
        [SerializeField] protected UniGlow.MultiLanguageText archiveNo;
        [SerializeField] protected UniGlow.MultiLanguageText coords;
        [SerializeField] protected TextMeshProUGUI pageNo;

        [Header("Animations")]
        [SerializeField] protected float tweenDuration = 1f;
        [SerializeField] protected Ease tweenEase = Ease.InOutCubic;

        // Protected
        protected Color originalBackgroundColor;
        protected List<POI> poiList;
        protected int poiListPos = 0;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        protected virtual void Start()
        {
            originalBackgroundColor = background.color;

            panel.SetActive(false);
            background.gameObject.SetActive(false);
            buttonLeft.SetActive(false);
            buttonRight.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            GameEvents.OnPoiGroupClicked += Show;
        }

        protected virtual void OnDisable()
        {
            GameEvents.OnPoiGroupClicked -= Show;
        }

        protected virtual void Update()
        {
            if (panel.activeSelf && (Input.GetButtonDown(Constants.INPUT_CANCEL) || Input.GetButtonDown(Constants.INPUT_QUIT))) Hide();
        }
        #endregion
        


        #region Public Functions
        public virtual void Show(List<POI> pPoiList)
        {
            // Set up all components
            poiList = pPoiList;
            poiListPos = 0;
            ReadData(poiList, 0);
            pageNo.text = 1 + " / " + poiList.Count;

            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, tweenDuration).SetEase(tweenEase);

            if (poiList.Count > 1) buttonLeft.SetActive(true);
            if (poiList.Count > 1) buttonRight.SetActive(true);

            background.gameObject.SetActive(true);
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
            background.DOColor(originalBackgroundColor, tweenDuration).SetEase(Ease.OutCubic);
        }

        public virtual void Hide()
        {
            DOTween.Kill(panel);
            DOTween.Kill(background);

            panel.transform.DOScale(Vector3.zero, tweenDuration).OnComplete(() => 
            {
                panel.SetActive(false);
            });

            buttonLeft.SetActive(false);
            buttonRight.SetActive(false);

            background.DOColor(new Color(0f,0f,0f,0f), tweenDuration).SetEase(Ease.InCubic).OnComplete(() => 
            {
                background.color = originalBackgroundColor;
                background.gameObject.SetActive(false); 
                GameEvents.PoiDescriptionClosed();
            });
        }

        public virtual void ReadData(List<POI> pPoiList, int pListPos)
        {
            DataSet data = pPoiList[pListPos].GetData();
            description.SetTexts(data.Description, data.DescriptionEnglish, data.DescriptionPlatt);
            author.SetTexts(data.Author, data.Author, data.Author);
            date.SetTexts(data.Date, data.DateEnglish, data.Date);
            archiveNo.SetTexts(data.FileName, data.FileName, data.FileName);
            coords.SetTexts(data.LatiAndLongitude, data.LatiAndLongitude, data.LatiAndLongitude);
        }

        public virtual void Next(bool right)
        {
            poiListPos = right ? (poiListPos + 1) % poiList.Count : ((poiListPos - 1) % poiList.Count + poiList.Count) % poiList.Count;
            ReadData(poiList, poiListPos);
            pageNo.text = poiListPos + 1 + " / " + poiList.Count;
        }

        public virtual void OnGalleryClicked(POIGallery pGallery)
        {
            poiListPos = (poiListPos + pGallery.GalleryPosition) % poiList.Count;
            ReadData(poiList, poiListPos);
            pageNo.text = poiListPos + 1 + " / " + poiList.Count;
        }
        #endregion



        #region Private Functions
        
        #endregion



        #region Coroutines

        #endregion
    }
}