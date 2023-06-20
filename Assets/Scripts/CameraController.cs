using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UniGlow
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [Space]
        [SerializeField] bool active = true;

        [Header("Zooming")]
        [Tooltip("Orthogonal size of the camera when zoomed in.")]
        [SerializeField] protected float zoomInSize = 200f;
        [SerializeField] protected float zoomTime = 1.5f;

        [Header("Dragging")]
        [SerializeField] protected float dragSpeed = 300;
        [SerializeField] protected Vector2 minDragPosition;
        [SerializeField] protected Vector2 maxDragPosition;

        // Private
        protected new Camera camera;
        protected float zoomOutSize;
        protected Vector3 dragOrigin;
        protected Vector3 cameraOrigin;
        protected bool isZoomedOut = true;
        protected Animation animation;
        #endregion



        #region Public Properties
        
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            camera = transform.GetRequiredComponent<Camera>();
            animation = GetComponent<Animation>();
            zoomOutSize = camera.orthographicSize;
        }

        private void Update()
        {
            if (!active) return;

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                cameraOrigin = transform.position;
                return;
            }

            if (!Input.GetMouseButton(0) || isZoomedOut) return;

            Drag();
        }
        #endregion



        #region Public Functions
        public void ZoomIn(Vector2 position = new Vector2())
        {
            Camera.main.transform.DOMove(new Vector3(position.x, position.y, Camera.main.transform.position.z), zoomTime);
            Camera.main.DOOrthoSize(zoomInSize, zoomTime).OnComplete(() =>
            {
                isZoomedOut = false;
            });
        }
        public void ZoomIn(bool ignoreIfCameraIsActive)
        {
            if (!ignoreIfCameraIsActive && !active) return;

            ZoomIn(Vector2.zero);
        }

        public void ZoomOut(bool ignoreIfCameraIsActive)
        {
            if (!ignoreIfCameraIsActive && !active) return;

            Camera.main.transform.DOMove(new Vector3(0f, 0f, Camera.main.transform.position.z), zoomTime);
            Camera.main.DOOrthoSize(zoomOutSize, zoomTime).OnComplete(() =>
            {
                isZoomedOut = true;
            });
        }
        public void ZoomOut()
        {
            ZoomOut(true);
        }

        public void ActivateControl(bool active)
        {
            this.active = active;
        }

        public void Animate(AnimationClip animationClip)
        {
            if (!animation) return;

            if (isZoomedOut) ZoomIn();

            animation.clip = animationClip;
            animation.enabled = true;
            animation.Play();
            StartCoroutine(ResetAnimationComponent());
        }
        #endregion



        #region Private Functions
        protected void Drag()
        {
            Vector3 newPosition = dragOrigin - Input.mousePosition;
            newPosition = cameraOrigin + new Vector3(newPosition.x * dragSpeed, newPosition.y * dragSpeed, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, minDragPosition.x, maxDragPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minDragPosition.y, maxDragPosition.y);
            transform.position = newPosition;
        }
        #endregion



        #region Coroutines
        IEnumerator ResetAnimationComponent()
        {
            yield return new WaitForSeconds(animation.clip.length);
            animation.Stop();
            animation.clip = null;
            animation.enabled = false;
        }
        #endregion
    }
}