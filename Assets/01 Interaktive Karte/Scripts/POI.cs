using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap
{
    public class POI : MonoBehaviour
    {
        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Button button;
        [SerializeField] Image image;

        // Private
        DataSet data;
        #endregion



        #region Public Properties

        #endregion


        #region Unity Event Functions

        #endregion


        #region Public Functions
        public void Initialize(DataSet dataSet)
        {
            data = dataSet;
        }

        /*public void OnClick()
        {
            GameEvents.PoiClicked(data);
        }*/

        public void Show()
        {
            button.enabled = true;
            image.enabled = true;
        }

        public void Hide()
        {
            button.enabled = false;
            image.enabled = false;
        }

        public Image getImage()
        {
            return image;
        }
        
        public DataSet GetData()
        {
            return data;
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}