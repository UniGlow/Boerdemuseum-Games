using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    #region Inputs
    public static readonly string INPUT_HORIZONTAL = "Horizontal";
    public static readonly string INPUT_VERTICAL = "Vertical";
    public static readonly string INPUT_FIRE1 = "Fire1";
    public static readonly string INPUT_MOUSE_X = "Mouse X";
    public static readonly string INPUT_MOUSE_Y = "Mouse Y";
    public static readonly string INPUT_JUMP = "Jump";
    public static readonly string INPUT_SUBMIT = "Submit";
    public static readonly string INPUT_CANCEL = "Cancel";
    public static readonly string INPUT_QUIT = "Quit";
    public static readonly string INPUT_DEBUGMODE = "Fire3";
    #endregion

    #region Tags and Layers
    public static readonly string TAG_PLAYER = "Player";

    // Tags for the mafia shootout (station 3)
    public static readonly string TAG_CANNONBALL = "Cannonball";
    public static readonly string TAG_SHIP = "Ship";
    public static readonly string TAG_WATER = "Water";
    #endregion

    #region Audio
    // Exposed Parameters in Mixers
    public static readonly string MIXER_VOICE_VOLUME = "VoiceVolume";
    public static readonly string MIXER_SFX_VOLUME = "SFXVolume";
    public static readonly string MIXER_MUSIC_VOLUME = "MusicVolume";
    #endregion

    #region Languages
    public static readonly string LANGUAGE_GERMAN = "German";
    public static readonly string LANGUAGE_ENGLISH = "English";
    public static readonly string LANGUAGE_PLATT = "Platt";
    #endregion
}
