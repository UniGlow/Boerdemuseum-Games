using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using DG.Tweening;
using UnityEngine.Events;

namespace ObjectMemory
{
    public class PhoneCamera : MonoBehaviour
    {

        #region Variable Declarations
        public static PhoneCamera Instance;

        [Space]
        [SerializeField] bool startAutomatically = true;
        [SerializeField] bool rotateCameraImage = true;

        [Space]
        [SerializeField] UnityEvent onTakingScreenshot;

        [Header("References")]
        [SerializeField] RawImage background;
        [SerializeField] AspectRatioFitter aspectRatioFitter;
        [SerializeField] GameObject screenshotEffect;

        [Header("Debugging")]
        [SerializeField] bool verbose;

        // Serialized Fields

        // Private
        WebCamTexture camTexture;
        bool startFrontFacingCamera;
        #endregion



        #region Public Properties
        public bool IsPlaying
        {
            get
            {
                if (camTexture == null) return false;
                else return camTexture.isPlaying;
            }
        }
        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);
        }

        private void Start()
        {
            background.enabled = false;
            if (screenshotEffect) screenshotEffect.SetActive(false);

            if (startAutomatically) StartCamera();
        }
        #endregion



        #region Public Functions
        public void StartCamera(bool frontFacing = true)
        {
            if (IsPlaying)
            {
                if (verbose) Debug.LogWarning("Phone Camera already playing. Aborting Start().");
                return;
            }

#if PLATFORM_ANDROID
        // Ask for user permission if necessary
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            startFrontFacingCamera = frontFacing;
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
            callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
            callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
            Permission.RequestUserPermission(Permission.Camera, callbacks);
            return;
        }
#elif PLATFORM_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            StartCoroutine(WaitForiOSResponse());
        }
#endif
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length == 0)
            {
                Debug.LogWarning("No camera detected.");
                return;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].isFrontFacing == frontFacing)
                {
                    if (verbose) Debug.Log("Screen dimensions: " + Screen.width + "x" + Screen.height);
                    camTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }

            if (camTexture == null)
            {
                if (!frontFacing) Debug.LogWarning("Unable to find back camera.");
                else Debug.LogWarning("Unable to find front camera.");
                return;
            }

            camTexture.Play();
            background.enabled = true;
            background.texture = camTexture;
            if (rotateCameraImage) background.rectTransform.localEulerAngles = new Vector3(0f, 0f, -camTexture.videoRotationAngle);

            StartCoroutine(Wait(3, () =>
            {
                float ratio = (float)camTexture.width / (float)camTexture.height;
                if (verbose) Debug.Log("cam width: " + camTexture.width + " cam height: " + camTexture.height);
                aspectRatioFitter.aspectRatio = ratio;
            }));
        }

        public void FreezeToSnapshot()
        {
            if (camTexture == null)
            {
                Debug.LogError("Can't take snapshot of camera image. No camera available.", gameObject);
                return;
            }

            Texture2D snap = new Texture2D(camTexture.width, camTexture.height);
            snap.SetPixels(camTexture.GetPixels());
            snap.Apply();

            camTexture.Stop();
            background.texture = snap;
        }

        public void StopCamera()
        {
            if (camTexture == null)
            {
                Debug.LogError("There was no camera initiated. Aborting StopCamera().", gameObject);
                return;
            }

            camTexture.Stop();
            background.enabled = false;
        }
        #endregion



        #region Private Functions
        internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
        }

        internal void PermissionCallbacks_PermissionGranted(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
            StartCamera(startFrontFacingCamera);
        }

        internal void PermissionCallbacks_PermissionDenied(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
        }
        #endregion



        #region Coroutines
        IEnumerator Wait(int frames, System.Action onComplete)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }

            onComplete.Invoke();
        }

        IEnumerator WaitForiOSResponse()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam)) StartCamera();
        }
        #endregion
    }
}