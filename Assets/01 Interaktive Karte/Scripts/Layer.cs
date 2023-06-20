using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap
{
    [System.Serializable]
    public class Layer
    {
        public bool generateNewRandomSets = false;
        public int numberOfRandomSets = 300;
        public DataSetList dataSetList;
        public List<POI> guiPoiList;
        public List<POIGroup> guiPoiGroupListZoomedIn;
        public List<POIGroup> guiPoiGroupListZoomedOut;
        public bool isActive = false;
        public Sprite layerImage;


        public void ShowGuiPoiGroups(bool zoomedOutList)
        {
            foreach (POIGroup lPoiGroup in zoomedOutList ? guiPoiGroupListZoomedOut : guiPoiGroupListZoomedIn)
            {
                lPoiGroup.Show();
            }
        }


        public void HideGuiPoiGroups()
        {
            foreach (POIGroup lPoiGroup in guiPoiGroupListZoomedOut)
            {
                lPoiGroup.Hide();
            }
            foreach (POIGroup lPoiGroup in guiPoiGroupListZoomedIn)
            {
                lPoiGroup.Hide();
            }
        }
        public void HideGuiPoiGroups(bool zoomedOutList)
        {
            foreach (POIGroup lPoiGroup in zoomedOutList ? guiPoiGroupListZoomedOut : guiPoiGroupListZoomedIn)
            {
                lPoiGroup.Hide();
            }
        }


        public void ToggleGuiPoiGroups(bool zoomedOutList)
        {
            foreach (POIGroup lPoiGroup in zoomedOutList ? guiPoiGroupListZoomedOut : guiPoiGroupListZoomedIn)
            {
                lPoiGroup.Show();
            }
            foreach (POIGroup lPoiGroup in zoomedOutList ? guiPoiGroupListZoomedIn : guiPoiGroupListZoomedOut)
            {
                lPoiGroup.Hide();
            }
        }
    }
}