using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InteractiveMap
{
    public class POIGallery : MonoBehaviour
    {
        #region Variable Declarations
        [SerializeField] Button yourButton;
        [SerializeField] Image yourImage;
        [SerializeField] TextMeshProUGUI yourCopyright;
        [SerializeField] GameObject highlightBorder;

        // Private
        POIDescriptionControllerImage poiImage;
        int galleryPosition;
        #endregion



        #region Public Properties
        public int GalleryPosition { get => galleryPosition; }
        #endregion



        #region Unity Event Functions
        void Start()
        {
            yourButton.onClick.AddListener(TaskOnClick);
        }
        #endregion



        #region Public Functions
        public void Initialize(POIDescriptionControllerImage poiImage, Transform parent, int galleryPosition)
        {
            this.poiImage = poiImage;
            this.galleryPosition = galleryPosition;

            transform.SetParent(parent, false);
            transform.localScale = Vector3.one;
        }

        public void SetImage(Sprite sprite, string copyright)
        {
            yourImage.sprite = sprite;
            yourCopyright.text = copyright;
            Highlight(false);
        }

        public void Highlight(bool lightsOn)
        {
            highlightBorder.SetActive(lightsOn);
        }
        #endregion



        #region Private Functions
        void TaskOnClick()
        {
            poiImage?.OnGalleryClicked(this);
        }
        #endregion



        #region Coroutines

        #endregion
    }
}
