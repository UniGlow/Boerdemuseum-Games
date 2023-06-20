using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace InteractiveMap
{
    public class POIManager : MonoBehaviour
    {
        #region Variable Declarations
        public static POIManager Instance;

        // Serialized Fields
        [SerializeField] MapSettings mapSettings;
        [SerializeField] Vector2 minMaxCanvasX; // -> longitude
        [SerializeField] Vector2 minMaxCanvasY; // -> latitude
        [SerializeField] GameObject poiPrefab;
        [SerializeField] GameObject poiGroupPrefab;
        [SerializeField] float groupingDistanceZoomedIn;
        [SerializeField] float groupingDistanceZoomedOut;
        [SerializeField] float locationGroupsGroupingDistance;
        [SerializeField] float imageScaleZoomedIn;
        [SerializeField] int minPoisForLargePoiGroup;
        [SerializeField] float smallPoiGroupSizeScale;
        [SerializeField] float maxPoiGroupScale;
        [SerializeField] int numberOfGroupedPoisAtMaxPoiGroupScale;
        #endregion
        


        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            SpawnPois();
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions
        void SpawnPois()
        {
            // Generate random GPSCoordsSets
            foreach (Layer layer in LayerManager.Instance.Layers)
            {
                if (layer.generateNewRandomSets) layer.dataSetList.NewRandomSets(layer.numberOfRandomSets, mapSettings.MinMaxLatitude.x, mapSettings.MinMaxLatitude.y, mapSettings.MinMaxLongitude.x, mapSettings.MinMaxLongitude.y);

                // Instantiate all POIs
                foreach (DataSet dataSet in layer.dataSetList.Sets)
                {
                    RectTransform poiTransform = Instantiate(poiPrefab, transform).GetComponent<RectTransform>();
                    poiTransform.anchoredPosition = MapGeoToGui(dataSet.GpsCoords.latitude.ToDegreeDecimal(), dataSet.GpsCoords.longitude.ToDegreeDecimal());
                    poiTransform.GetComponent<POI>().Initialize(dataSet);
                    poiTransform.GetComponentInChildren<Image>().sprite = layer.layerImage;
                    poiTransform.GetComponent<POI>().Hide();
                    layer.guiPoiList.Add(poiTransform.GetComponent<POI>());
                }

                layer.guiPoiGroupListZoomedOut = GroupPois(layer.guiPoiList, groupingDistanceZoomedOut);
                layer.guiPoiGroupListZoomedIn = InstantiateLocationPoiGroups(layer.guiPoiList, groupingDistanceZoomedIn);

                foreach (POIGroup poiGroup in layer.guiPoiGroupListZoomedOut)
                {
                    poiGroup.setSprite(layer.layerImage);
                    float poiGroupScale = poiGroup.GroupedPois.Count < minPoisForLargePoiGroup ? smallPoiGroupSizeScale : 1;
                    //float poiGroupScale = poiGroup.GroupedPois.Count >= numberOfGroupedPoisAtMaxPoiGroupScale ? maxPoiGroupScale : (float)poiGroup.GroupedPois.Count / (float)numberOfGroupedPoisAtMaxPoiGroupScale * (maxPoiGroupScale - 1) + 1;
                    poiGroup.GetComponent<RectTransform>().localScale = new Vector3(poiGroupScale, poiGroupScale, poiGroupScale);
                    poiGroup.setText(""); //poiGroup.GroupedPois.Count.ToString());
                }

                foreach (POIGroup poiGroup in layer.guiPoiGroupListZoomedIn)
                {
                    poiGroup.setSprite(layer.layerImage);
                    float poiGroupScale = poiGroup.GroupedPois.Count < minPoisForLargePoiGroup ? smallPoiGroupSizeScale : 1;
                    //float poiGroupScale = poiGroup.GroupedPois.Count >= numberOfGroupedPoisAtMaxPoiGroupScale ? maxPoiGroupScale : ((float)poiGroup.GroupedPois.Count / (float)numberOfGroupedPoisAtMaxPoiGroupScale * (maxPoiGroupScale - 1) + 1);
                    poiGroup.GetComponent<RectTransform>().localScale = new Vector3(poiGroupScale, poiGroupScale, poiGroupScale) * imageScaleZoomedIn;
                    poiGroup.setText(""); //poiGroup.GroupedPois.Count.ToString());
                }
            }

            LayerManager.Instance.SetActiveLayer(0);
        }


        List<POIGroup> GroupPois(List<POI> pPoiList, float pGroupingDistance)
        {
            float[,] lPoiDistanceMatrix = CalculatePoiDistance(pPoiList, pGroupingDistance);
            List<List<float>> lPoiDistanceLists = GeneratePoiDistanceLists(lPoiDistanceMatrix);
            return InstantiatePoiGroups(pPoiList, lPoiDistanceMatrix, lPoiDistanceLists, pGroupingDistance);
        }


        float[,] CalculatePoiDistance(List<POI> pPoiList, float pGroupingDistance)
        {
            int layerPOICount = pPoiList.Count;
            float[,] poiDistanceMatrix = new float[layerPOICount, layerPOICount];

            for (int i = 0; i < layerPOICount - 1; i++)
            {
                for (int j = i + 1; j < layerPOICount; j++)
                {
                    if (pPoiList[i].GetData().Radius != "" && pPoiList[j].GetData().Radius != "" && pPoiList[i].GetData().Radius == pPoiList[j].GetData().Radius)
                    {
                        poiDistanceMatrix[i, j] = 0;
                    }
                    else
                    {
                        float twoPOIDistance = Vector2.Distance(pPoiList[i].GetComponent<RectTransform>().anchoredPosition, pPoiList[j].GetComponent<RectTransform>().anchoredPosition);
                        poiDistanceMatrix[i, j] = (twoPOIDistance < pGroupingDistance) ? twoPOIDistance : -1;
                    }
                }
            }

            return poiDistanceMatrix;
        }


        List<List<float>> GeneratePoiDistanceLists(float[,] pDistanceMatrix)
        {
            List<List<float>> poiDistanceLists = new List<List<float>>();
            int pDistanceArrayLength = pDistanceMatrix.GetLength(0);

            for (int i = 0; i < pDistanceArrayLength; i++)
            {
                poiDistanceLists.Add(new List<float>());
            }

            for (int i = 0; i < pDistanceArrayLength - 1; i++)
            {
                for (int j = i + 1; j < pDistanceArrayLength; j++)
                {
                    if (pDistanceMatrix[i, j] >= 0)
                    {
                        poiDistanceLists[i].Add(pDistanceMatrix[i, j]);
                        poiDistanceLists[j].Add(pDistanceMatrix[i, j]);
                    }
                }
            }

            return poiDistanceLists;
        }


        List<POIGroup> InstantiatePoiGroups(List<POI> pPoiList, float[,] pDistanceMatrix, List<List<float>> pPoiDistanceLists, float pGroupingDistance)
        {
            RectTransform poiGroupTransform = null;
            List<POIGroup> lGuiPoiGroupList = new List<POIGroup>();


            // Instantiate corresponding GroupPOI with 1 POI
            for (int i = 0; i < pPoiDistanceLists.Count; i++)
            {
                if (pPoiDistanceLists[i].Count == 0)
                {
                    poiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                    poiGroupTransform.anchoredPosition = pPoiList[i].GetComponent<RectTransform>().anchoredPosition;
                    poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[i].GetComponent<POI>());
                    pPoiList[i].gameObject.SetActive(false);
                    lGuiPoiGroupList.Add(poiGroupTransform.GetComponent<POIGroup>());
                }
            }


            // Instantiate corresponding GroupPOI with >2 POIs
            while (true)
            {
                int biggestPoiGroupValue = 1;
                List<int> biggestPoiGroupPositions = new List<int>();

                // Do any GroupPOIs with >2 POIs exist?
                for (int i = 0; i < pPoiDistanceLists.Count; i++)
                {
                    if (pPoiDistanceLists[i].Count > biggestPoiGroupValue)
                    {
                        biggestPoiGroupPositions = new List<int>();
                        biggestPoiGroupValue = pPoiDistanceLists[i].Count;
                    }
                    if (pPoiDistanceLists[i].Count == biggestPoiGroupValue)
                    {
                        biggestPoiGroupPositions.Add(i);
                    }
                }
                if (biggestPoiGroupValue < 2) break;

                // Calculate most dense GroupPOI
                List<float> poiGroupDensities = new List<float>();

                for (int i = 0; i < biggestPoiGroupPositions.Count; i++)
                {
                    float poiGroupDensity = 0;
                    foreach (float poiDistance in pPoiDistanceLists[biggestPoiGroupPositions[i]])
                    {
                        poiGroupDensity += poiDistance;
                    }

                    poiGroupDensities.Add(poiGroupDensity / biggestPoiGroupValue);
                }

                float lowestGroupDensity = pGroupingDistance;
                foreach (float groupDensity in poiGroupDensities)
                {
                    lowestGroupDensity = groupDensity < lowestGroupDensity ? groupDensity : lowestGroupDensity;
                }


                int listPositionToInstantiate = biggestPoiGroupPositions[poiGroupDensities.IndexOf(lowestGroupDensity)];

                poiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                poiGroupTransform.anchoredPosition = pPoiList[listPositionToInstantiate].GetComponent<RectTransform>().anchoredPosition;
                poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[listPositionToInstantiate].GetComponent<POI>());

                for (int i = 0; i < pDistanceMatrix.GetLength(0); i++)
                {
                    if ((i < listPositionToInstantiate) && (pDistanceMatrix[i, listPositionToInstantiate] >= 0))
                    {
                        poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[i].GetComponent<POI>());
                        pDistanceMatrix[i, listPositionToInstantiate] = -1;

                        RemoveGroupedPoisFromLists(pDistanceMatrix, i, pPoiDistanceLists, pPoiList, lGuiPoiGroupList);
                    }
                    else if ((i > listPositionToInstantiate) && (pDistanceMatrix[listPositionToInstantiate, i] >= 0))
                    {
                        poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[i].GetComponent<POI>());
                        pDistanceMatrix[listPositionToInstantiate, i] = -1;

                        RemoveGroupedPoisFromLists(pDistanceMatrix, i, pPoiDistanceLists, pPoiList, lGuiPoiGroupList);
                    }
                }
                pPoiList[listPositionToInstantiate].gameObject.SetActive(false);
                lGuiPoiGroupList.Add(poiGroupTransform.GetComponent<POIGroup>());

                pPoiDistanceLists[listPositionToInstantiate] = new List<float>();
            }


            // Instantiate corresponding GroupPOI with 2 POIs
            for (int i = 0; i < pPoiDistanceLists.Count; i++)
            {
                if (pPoiDistanceLists[i].Count == 1)
                {
                    poiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                    poiGroupTransform.anchoredPosition = pPoiList[i].GetComponent<RectTransform>().anchoredPosition;
                    poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[i].GetComponent<POI>());

                    for (int j = 0; j < pDistanceMatrix.GetLength(0); j++)
                    {
                        if ((j < i) && (pDistanceMatrix[j, i] == pPoiDistanceLists[i][0]))
                        {
                            poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[j].GetComponent<POI>());

                            pPoiDistanceLists[i] = new List<float>();
                            pPoiDistanceLists[j] = new List<float>();
                            pDistanceMatrix[j, i] = -1;

                            j = pDistanceMatrix.GetLength(0);
                        }
                        else if ((j > i) && (pDistanceMatrix[i, j] == pPoiDistanceLists[i][0]))
                        {
                            poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[j].GetComponent<POI>());

                            pPoiDistanceLists[i] = new List<float>();
                            pPoiDistanceLists[j] = new List<float>();
                            pDistanceMatrix[i, j] = -1;

                            j = pDistanceMatrix.GetLength(0);
                        }
                    }
                    pPoiList[i].gameObject.SetActive(false);
                    lGuiPoiGroupList.Add(poiGroupTransform.GetComponent<POIGroup>());
                }
            }

            return lGuiPoiGroupList;
        }


        void RemoveGroupedPoisFromLists(float[,] pDistanceMatrix, int pivot, List<List<float>> pPoiDistanceLists, List<POI> pPoiList, List<POIGroup> pGuiPoiGroupList)
        {
            RectTransform poiGroupTransform;

            for (int k = 0; k < pDistanceMatrix.GetLength(0); k++)
            {
                if ((k < pivot) && (pDistanceMatrix[k, pivot] >= 0))
                {
                    pPoiDistanceLists[k].Remove(pDistanceMatrix[k, pivot]);
                    if (pPoiDistanceLists[k].Count == 0)
                    {
                        poiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                        poiGroupTransform.anchoredPosition = pPoiList[k].GetComponent<RectTransform>().anchoredPosition;
                        poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[k].GetComponent<POI>());
                        pPoiList[k].gameObject.SetActive(false);
                        pGuiPoiGroupList.Add(poiGroupTransform.GetComponent<POIGroup>());
                    }
                    pDistanceMatrix[k, pivot] = -1;
                }
                else if ((k > pivot) && (pDistanceMatrix[pivot, k] >= 0))
                {
                    pPoiDistanceLists[k].Remove(pDistanceMatrix[pivot, k]);
                    if (pPoiDistanceLists[k].Count == 0)
                    {
                        poiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                        poiGroupTransform.anchoredPosition = pPoiList[k].GetComponent<RectTransform>().anchoredPosition;
                        poiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(pPoiList[k].GetComponent<POI>());
                        pPoiList[k].gameObject.SetActive(false);
                        pGuiPoiGroupList.Add(poiGroupTransform.GetComponent<POIGroup>());
                    }
                    pDistanceMatrix[pivot, k] = -1;
                }
            }

            pPoiDistanceLists[pivot] = new List<float>();
        }


        List<POIGroup> InstantiateLocationPoiGroups(List<POI> pPoiList, float pGroupingDistance)
        {
            RectTransform lPoiGroupTransform;
            List<POIGroup> lGuiPoiGroupList = new List<POIGroup>();
            List<string> lLocations = new List<string>();
            List<POI> lLocationlessPois = new List<POI>();

            foreach (POI lPOI in pPoiList)
            {
                if (lPOI.GetData().Radius != "")
                {
                    int lLocIndex = lLocations.IndexOf(lPOI.GetData().Radius);

                    if (lLocIndex < 0)
                    {
                        lLocations.Add(lPOI.GetData().Radius);

                        lPoiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                        lPoiGroupTransform.anchoredPosition = lPOI.GetComponent<RectTransform>().anchoredPosition;
                        lPoiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(lPOI.GetComponent<POI>());
                        lPOI.gameObject.SetActive(false);
                        lGuiPoiGroupList.Add(lPoiGroupTransform.GetComponent<POIGroup>());
                    }
                    else
                    {
                        lGuiPoiGroupList[lLocIndex].GroupedPois.Add(lPOI.GetComponent<POI>());
                    }
                }
                else
                {
                    lLocationlessPois.Add(lPOI);
                }
            }

            foreach (POI lPOI in lLocationlessPois.ToList())
            {
                if (lLocationlessPois.Contains(lPOI))
                {
                    string lNearestLocation = "";
                    float lNearestLocationGap = pGroupingDistance;

                    foreach (POI lIsNearestPoi in pPoiList)
                    {
                        if (lIsNearestPoi.GetData().Radius != "")
                        {
                            float twoPOIDistance = Vector2.Distance(lPOI.GetComponent<RectTransform>().anchoredPosition, lIsNearestPoi.GetComponent<RectTransform>().anchoredPosition);
                            if (twoPOIDistance < lNearestLocationGap && twoPOIDistance > 0)
                            {
                                lNearestLocation = lIsNearestPoi.GetData().Radius;
                                lNearestLocationGap = twoPOIDistance;
                            }
                        }
                    }

                    if (lNearestLocationGap < pGroupingDistance)
                    {
                        lGuiPoiGroupList[lLocations.IndexOf(lNearestLocation)].GroupedPois.Add(lPOI.GetComponent<POI>());
                        lLocationlessPois.Remove(lPOI);
                    }
                }
            }

            foreach (POI lPOI in lLocationlessPois.ToList())
            {
                if (lLocationlessPois.Contains(lPOI))
                {
                    lPoiGroupTransform = Instantiate(poiGroupPrefab, transform).GetComponent<RectTransform>();
                    lPoiGroupTransform.anchoredPosition = lPOI.GetComponent<RectTransform>().anchoredPosition;
                    lPoiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(lPOI.GetComponent<POI>());
                    lPOI.gameObject.SetActive(false);
                    lGuiPoiGroupList.Add(lPoiGroupTransform.GetComponent<POIGroup>());
                    lLocationlessPois.Remove(lPOI);

                    foreach (POI lIsPoiNearby in lLocationlessPois.ToList())
                    {
                        if (lLocationlessPois.Contains(lIsPoiNearby))
                        {
                            float twoPOIDistance = Vector2.Distance(lPOI.GetComponent<RectTransform>().anchoredPosition, lIsPoiNearby.GetComponent<RectTransform>().anchoredPosition);
                            if (twoPOIDistance < pGroupingDistance)
                            {
                                lPoiGroupTransform.gameObject.GetComponent<POIGroup>().GroupedPois.Add(lIsPoiNearby.GetComponent<POI>());
                                lLocationlessPois.Remove(lIsPoiNearby);
                            }
                        }
                    }
                }
            }

            return lGuiPoiGroupList; // GroupLocationPoiGroups(lGuiPoiGroupList, locationGroupsGroupingDistance);
        }


        List<POIGroup> GroupLocationPoiGroups(List<POIGroup> pPoiGroupList, float pGroupingDistance)
        {
            int lPoiGroupsCount = pPoiGroupList.Count;
            List<float[]> lGroupsToBeGrouped = new List<float[]>();
            List<int> lPoiGroupsIDList = new List<int>();

            for (int i = 0; i < lPoiGroupsCount - 1; i++)
            {
                for (int j = i + 1; j < lPoiGroupsCount; j++)
                {
                    float twoPoiDistance = Vector2.Distance(pPoiGroupList[i].GetComponent<RectTransform>().anchoredPosition, pPoiGroupList[j].GetComponent<RectTransform>().anchoredPosition);
                    if (twoPoiDistance < pGroupingDistance)
                    {
                        lGroupsToBeGrouped.Add(new float[] { twoPoiDistance, i * lPoiGroupsCount + j });

                        if (!lPoiGroupsIDList.Contains(i)) { lPoiGroupsIDList.Add(i); }
                        if (!lPoiGroupsIDList.Contains(j)) { lPoiGroupsIDList.Add(j); }
                    }
                }
            }

            while (true)
            {
                float lNearestGroups = pGroupingDistance;
                int lCombinedIDs = 0;
                int lIDi;
                int lIDj;
                for (int i = 0; i < lGroupsToBeGrouped.Count; i++)
                {
                    lCombinedIDs = (int)lGroupsToBeGrouped[i][1];
                    lIDi = lCombinedIDs / lPoiGroupsCount;
                    lIDj = lCombinedIDs % lPoiGroupsCount;

                    if (lPoiGroupsIDList.Contains(lIDi) && lPoiGroupsIDList.Contains(lIDj))
                    {
                        if (lGroupsToBeGrouped[i][0] < lNearestGroups)
                        {
                            lNearestGroups = lGroupsToBeGrouped[i][0];
                            lCombinedIDs = (int)lGroupsToBeGrouped[i][1];
                        }
                    }
                    else
                    {
                        lGroupsToBeGrouped.RemoveAt(i);
                    }
                }

                if (lNearestGroups == pGroupingDistance)
                {
                    break;
                }

                lIDi = lCombinedIDs / lPoiGroupsCount;
                lIDj = lCombinedIDs % lPoiGroupsCount;

                if (pPoiGroupList[lIDi].GroupedPois.Count < pPoiGroupList[lIDj].GroupedPois.Count)
                {
                    pPoiGroupList[lIDj].GroupedPois.Concat(pPoiGroupList[lIDi].GroupedPois);
                    lPoiGroupsIDList.Remove(lIDi);
                }
                else
                {
                    pPoiGroupList[lIDi].GroupedPois.Concat(pPoiGroupList[lIDj].GroupedPois);
                    lPoiGroupsIDList.Remove(lIDj);
                }
            }

            return pPoiGroupList;
        }


        Vector2 MapGeoToGui(double latitude, double longitude)
        {
            Vector2 guiCoords = new Vector2();

            guiCoords.x = ((float)longitude).Remap(mapSettings.MinMaxLongitude.x, mapSettings.MinMaxLongitude.y, minMaxCanvasX.x, minMaxCanvasX.y);
            guiCoords.y = ((float)latitude).Remap(mapSettings.MinMaxLatitude.x, mapSettings.MinMaxLatitude.y, minMaxCanvasY.x, minMaxCanvasY.y);

            return guiCoords;
        }
        #endregion



        #region Coroutines

        #endregion
    }
}