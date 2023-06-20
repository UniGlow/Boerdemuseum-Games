using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Wimmelbild
{
    /// <summary>
    /// ScriptableObject containing a list of messages of one dialogue.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue.asset", menuName = "Scriptable Objects/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        #region Variable Declarations
        [Space]
        public Sprite ComplementingImage;

        [Space]
        public Message[] Messages;
        #endregion
    }
}