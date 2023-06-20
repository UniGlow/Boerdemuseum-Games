using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Wimmelbild
{
    [RequireComponent(typeof(Image))]
    public class AudioMuteSwitch : MonoBehaviour
    {

        #region Variable Declarations
        // Serialized Fields
        [SerializeField] Sprite audioOn;
        [SerializeField] Sprite audioOff;

        [Space]
        [SerializeField] AudioMixer masterMixer;

        // Private
        Image buttonImage;
        bool audioMuted;
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            buttonImage = transform.GetRequiredComponent<Image>();
        }

        private void Start()
        {
            buttonImage.sprite = audioOn;
        }
        #endregion



        #region Public Functions
        public void SwitchAudioMute()
        {
            audioMuted = !audioMuted;

            if (audioMuted)
            {
                buttonImage.sprite = audioOff;
                masterMixer.FindSnapshot("Muted").TransitionTo(1f);
            }
            else
            {
                buttonImage.sprite = audioOn;
                masterMixer.FindSnapshot("Default").TransitionTo(1f);
            }
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}