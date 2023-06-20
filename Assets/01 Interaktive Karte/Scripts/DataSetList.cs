using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveMap
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Data Set List")]
    public class DataSetList : ScriptableObject
    {
        #region Variable Declarations
        // Public
        public List<DataSet> Sets = new List<DataSet>();

        #endregion



        #region Public Properties

        #endregion


        
        #region Public Functions
        public void NewRandomSets(int numberOfSets, float minLatitudeDecimal, float maxLatitudeDecimal, float minLongitudeDecimal, float maxLongitudeDecimal)
        {
            Sets.Clear();

            for (int i = 0; i < numberOfSets; i++)
            {
                DataSet lSet = new DataSet();
                GPSCoords lGPSCoords = new GPSCoords();
                lGPSCoords.latitude = NewRandomDMSFromDecimal(minLatitudeDecimal, maxLatitudeDecimal);
                lGPSCoords.longitude = NewRandomDMSFromDecimal(minLongitudeDecimal, maxLongitudeDecimal);
                lSet.GpsCoords = lGPSCoords;
                lSet.Description = "Set " + i;
                lSet.Author = "Owner " + i;
                lSet.FileName = "ArchiveNumber " + i;
                Sets.Add(lSet);
            }
        }
        #endregion



        #region Private Functions
        GPSCoords.DMS NewRandomDMSFromDecimal(float minDecimal, float maxDecimal)
        {
            GPSCoords.DMS lDMS = new GPSCoords.DMS();
            float lRandomDecimal = Random.Range(minDecimal, maxDecimal);
            lDMS.degrees = (int)lRandomDecimal;
            lDMS.minutes = (int)(lRandomDecimal % 1 * 60);
            lDMS.seconds = lRandomDecimal % 1 * 60 % 1 * 60;
            return lDMS;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}