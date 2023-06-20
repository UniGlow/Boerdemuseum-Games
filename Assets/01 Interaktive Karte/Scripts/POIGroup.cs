using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InteractiveMap
{
    public class POIGroup : MonoBehaviour
    {
        #region Variable Declarations
        // Public
        public List<POI> GroupedPois;

        // Serialized Fields
        [SerializeField] Button button;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI text;

        // Private

        #endregion



        #region Public Properties

        #endregion


        #region Unity Event Functions

        #endregion


        #region Public Functions
        public void OnClick()
        {
            GameEvents.PoiGroupClicked(GroupedPois);
        }

        public void Show()
        {
            button.enabled = true;
            button.GetComponent<Image>().enabled = true;
            image.enabled = true;
            text.enabled = true;
        }

        public void Hide(POIGroup poiGroup = null)
        {
            button.enabled = false;
            button.GetComponent<Image>().enabled = false;
            image.enabled = false;
            text.enabled = false;
        }

        public void setSprite(Sprite pSprite)
        {
            image.sprite = pSprite;
        }

        public void setText(string pText)
        {
            text.text = pText;
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}