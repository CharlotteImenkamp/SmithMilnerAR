using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible to play audioClips depending on GameState
/// Gets list of audioclips from ownData
/// </summary>
public class AudioManager : SubManager
{
    //private string[] audioClipAdresses;
    //private float audioVolume;

    public void Initialize()
    {
    }

    #region gameStates
    public override void OnGameStateEntered(string newState)
    {
        switch (newState)
        {
            case "Initialization":
                Initialize(); 
                break;
            case "SettingsMenu":
                break;
            case "LocationTest":
                PlayAudioClip(); 
                break;
            case "LocationEstimation":
                PlayAudioClip();
                break;
            case "PriceTest":
                PlayAudioClip();
                break;
            case "PriceEstimation":
                PlayAudioClip();
                break;
            case "Pause":
                PlayAudioClip();
                break;
            default:
                break;
        }
    }

    public override void OnGameStateLeft(string oldState)
    {
        //\TODO löschen, falls nichts mehr rein kommt
        switch (oldState)
        {
            case "Initialization":
                break;
            case "SettingsMenu":
                break;
            case "LocationTest":
                break;
            case "LocationEstimation":
                break;
            case "PriceTest":
                break;
            case "PriceEstimation":
                break;
            case "Pause":
                break;
            default:
                Debug.LogError("AudioManager::OnGameStateLeft invalid State.");
                break;
        }
    }

    #endregion gameStates

    public override void Reset()
    {
        //audioClipAdresses = null;
        //audioVolume = 0.0f; 
    }

    #region audioClip helper

    private void PlayAudioClip()
    {
        Debug.LogWarning("AudioManager::PlayAudioClip not implemented.");
    }

    private AudioClip[] LoadAudioClips(string[] audioClipAdresses)
    {
        Debug.LogWarning("AudioManager::LoadAudioClips not implemented.");

        return null; 
    }
    #endregion audioClip helper

}
