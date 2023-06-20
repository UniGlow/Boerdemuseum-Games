using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap
{
    public class LayerManager : MonoBehaviour
    {

        #region Variable Declarations
        public static LayerManager Instance;

        // Serialized Fields
        [SerializeField] List<Layer> layers = new List<Layer>();

        [Space]
        [SerializeField] List<Image> buttonImages = new List<Image>();
        [SerializeField] List<Sprite> activeButtonSprites = new List<Sprite>();
        [SerializeField] List<Sprite> inactiveButtonSprites = new List<Sprite>();

        // Private
        bool isZoomedOut = true;
        #endregion



        #region Public Properties
        public List<Layer> Layers { get => layers; }
        public List<Layer> ActiveLayers
        {
            get
            {
                List<Layer> activeLayers = new List<Layer>();
                foreach (Layer layer in layers)
                {
                    if (layer.isActive) activeLayers.Add(layer);
                }

                return activeLayers;
            }
        }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            // Setup buttons
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].isActive) buttonImages[i].color = Color.white;
                else buttonImages[i].sprite = inactiveButtonSprites[i];
            }
        }

        private void OnEnable()
        {
            GameEvents.OnZoomingIn += ZoomingIn;
            GameEvents.OnZoomingOut += ZoomingOut;
        }

        private void OnDisable()
        {
            GameEvents.OnZoomingIn -= ZoomingIn;
            GameEvents.OnZoomingOut -= ZoomingOut;
        }
        #endregion



        #region Public Functions
        public void ToggleLayer(int layerIndex)
        {
            if (layers[layerIndex].isActive) layers[layerIndex].HideGuiPoiGroups(isZoomedOut);
            else layers[layerIndex].ShowGuiPoiGroups(isZoomedOut);

            buttonImages[layerIndex].sprite = layers[layerIndex].isActive ? inactiveButtonSprites[layerIndex] : activeButtonSprites[layerIndex];
            layers[layerIndex].isActive = !layers[layerIndex].isActive;
        }


        public void SetActiveLayer(int layerIndex)
        {
            if (layers.Count <= layerIndex) return;

            for (int i = 0; i < layers.Count; i++)
            {
                if (i == layerIndex)
                {
                    layers[i].ShowGuiPoiGroups(isZoomedOut);
                    layers[i].HideGuiPoiGroups(!isZoomedOut);

                    layers[i].isActive = true;
                }
                else
                {
                    layers[i].HideGuiPoiGroups();
                    layers[i].isActive = false;
                }
            }
        }


        void ZoomingIn()
        {
            foreach (Layer layer in layers)
            {
                if (layer.isActive)
                {
                    layer.ToggleGuiPoiGroups(false);
                }
                else
                {
                    layer.HideGuiPoiGroups();
                }
            }

            isZoomedOut = false;
        }


        void ZoomingOut()
        {
            foreach (Layer layer in layers)
            {
                if (layer.isActive)
                {
                    layer.ToggleGuiPoiGroups(true);
                }
                else
                {
                    layer.HideGuiPoiGroups();
                }
            }

            isZoomedOut = true;
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}