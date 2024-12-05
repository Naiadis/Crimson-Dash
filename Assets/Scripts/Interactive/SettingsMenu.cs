using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{public AudioMixer audioMixer;
public void SetMusicVolume (float sliderValue){
if (sliderValue == -80)
        {
            audioMixer.SetFloat("Music Volume", -80);
            return;
        }
        
        float volumePercent = (sliderValue + 80) / 80;
        float dbValue = (volumePercent * 40) - 40;
        
        audioMixer.SetFloat("Music Volume", dbValue);
    }
public void SetSFXVolume(float sliderValue){
if (sliderValue == -80)
        {
            audioMixer.SetFloat("SFX Volume", -80);
            return;
        }
        
        float volumePercent = (sliderValue + 80) / 80;
        float dbValue = (volumePercent * 40) - 40;
        
        audioMixer.SetFloat("SFX Volume", dbValue);}

public void SetQuality (int qualityIndex){
    QualitySettings.SetQualityLevel(qualityIndex);
}
}
