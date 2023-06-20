using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wimmelbild
{
    /// <summary>
    /// Data Structure defining the elements of a message of dialogues.
    /// </summary>
    [System.Serializable]
    public class Message
    {
        [Tooltip("Name of the talking person")]
        public Sprite CharacterName;
        [Tooltip("Image of the wanted person and emotion")]
        public Sprite CharacterImage;
        [Tooltip("Sound for the message")]
        public AudioClip Sound;
        public AudioClip SoundEnglish;
        public AudioClip SoundPlatt;
        [Tooltip("Volume for the sound")]
        [Range(0f, 1f)]
        public float SoundVolume = 1f;
        [Tooltip("Time (in seconds) the sound is offset from the beginning of showing the message")]
        [Range(0f, 10f)]
        public float SoundOffset;
        [Tooltip("Message that shall be displayed")]
        [TextArea(2, 5)]
        public string Text;
        [TextArea(2, 5)]
        public string TextEnglish;
        [TextArea(2, 5)]
        public string TextPlatt;
    }
}