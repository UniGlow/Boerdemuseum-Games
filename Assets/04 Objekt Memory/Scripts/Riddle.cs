using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectMemory
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Riddle")]
    public class Riddle : ScriptableObject
    {

        #region Variable Declarations
        [Header("Objektname")]
        public string Name;
        public string NameEnglish;
        public string NamePlatt;

        [Space]
        public Sprite Image;
        public float ZoomScale;
        public Vector2 ZoomLocalPosition;

        [Header("Lösungstext")]
        [TextArea(2, 3)]
        public string Text;
        [TextArea(2, 3)]
        public string TextEnglish;
        [TextArea(2, 3)]
        public string TextPlatt;

        [Space]
        public AudioClip Voiceover;

        [Space]
        public string Room;
        public string RoomEnglish;

        [Space]
        public string QRCode;
        #endregion



        #region Public Properties

        #endregion



        #region Public Functions

        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}