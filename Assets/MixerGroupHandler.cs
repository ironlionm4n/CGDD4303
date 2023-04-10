using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerGroupHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    public void HandleMusicVolumeChanged(Slider slider)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(slider.value) * 20);
    }

    public void HandleSFXVolumeChanged(Slider slider)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(slider.value) * 20);
    }
}