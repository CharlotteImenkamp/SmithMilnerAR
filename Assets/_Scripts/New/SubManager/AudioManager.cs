using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible to play audioClips depending on GameState
/// Gets list of audioclips from ownData
/// </summary>
public class AudioManager : SubManager
{
    private string[] audioClipAdresses;
    private float audioVolume;
    public void Initialize()
    {
        // loadAdresses
            // get filepath from GameManager
        //LoadAudioClips()
        Debug.Log("AudioManager Initialized."); 
    }

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
        Debug.LogWarning("AudioManager::OnGameStateEntered not implemented.");
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    public override void OnGameStateLeft(string oldState)
    {
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
                break;
        }
        Debug.LogWarning("AudioManager::OnGameStateLeft not implemented.");
    }

    private void PlayAudioClip()
    {
        Debug.LogWarning("AudioManager::PlayAudioClip not implemented.");
    }
    private AudioClip[] LoadAudioClips(string[] audioClipAdresses)
    {
        Debug.LogWarning("AudioManager::LoadAudioClips not implemented.");

        return null; 
    }
}
