using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace InteractiveMap
{
    public class POIDescriptionControllerAudio : POIDescriptionController
    {

        #region Variable Declarations
        // Serialized Fields
        [Header("Audio Description Specific")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI timer;
        [SerializeField] Button buttonPlay;
        [SerializeField] Sprite spritePlay;
        [SerializeField] Sprite spritePause;

        // Private

        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        override protected void Update()
        {
            base.Update();

            if (audioSource.clip == null) { return; }

            if (audioSource.time < audioSource.clip.length)
            {
                slider.value = audioSource.time / audioSource.clip.length;

                if (audioSource.time == 0)
                {
                    if (audioSource.isPlaying) buttonPlay.image.sprite = spritePause;
                    else if (buttonPlay.image.sprite != spritePlay) buttonPlay.image.sprite = spritePlay;
                }
            }

            string colon = ":";
            if ((int)audioSource.time % 60 < 10)
            {
                colon = ":0";
            }
            timer.text = ((int)audioSource.time / 60).ToString() + colon + ((int)audioSource.time % 60).ToString();
        }
        #endregion



        #region Public Functions
        public override void Show(List<POI> pPoiList)
        {
            // Only act when audio needs to be played
            if (!pPoiList[0].GetData().isAudio) return;

            base.Show(pPoiList);
            ReadData(poiList, 0);
        }

        public override void Hide()
        {
            audioSource.Stop();
            buttonPlay.image.sprite = spritePlay;
            base.Hide();
        }

        public override void ReadData(List<POI> pPoiList, int pListPos)
        {
            base.ReadData(pPoiList, pListPos);
            DataSet data = pPoiList[pListPos].GetData();

            audioSource.clip = data.Audio;
            audioSource.time = 0;
            audioSource.PlayDelayed(1);
        }

        public void ChangeAudioTime()
        {
            audioSource.time = audioSource.clip.length * slider.value;
        }

        public void PlayPause()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                buttonPlay.image.sprite = spritePlay;
            }
            else
            {
                audioSource.Play();
                buttonPlay.image.sprite = spritePause;
            }
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}