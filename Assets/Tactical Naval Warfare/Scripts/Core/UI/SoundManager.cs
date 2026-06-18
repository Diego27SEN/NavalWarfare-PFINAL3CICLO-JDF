using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SoundManager : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private string extraVolume = "ExtraVolume";
    [SerializeField] private string musicVolume = "MusicVolume";
    [SerializeField] private string sfxVolume = "SFXVolume";

    [Header("UI Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [FoldoutGroup("Music Tracks", expanded: false)]
    [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

    [FoldoutGroup("SFX Clips", expanded: false)]
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();

    [FoldoutGroup("Extras", expanded: false)]
    [SerializeField] private List<AudioClip> extraClips = new List<AudioClip>();

    void Start()
    {
        if(masterSlider != null)
        {
            masterSlider.onValueChanged.AddListener(SetExtraVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        UpdateSlidersFromMixer();
    }
    
    public void SetExtraVolume(float value)
    {
        audioMixer.SetFloat(extraVolume, Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicVolume, Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(sfxVolume, Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f);
    }

    private void UpdateSlidersFromMixer()
    {
        if (audioMixer == null) return;

        if (masterSlider != null && audioMixer.GetFloat(extraVolume, out float masterValue))
        {
            masterSlider.value = Mathf.Pow(10f, masterValue / 20f);
        }


        if (musicSlider != null && audioMixer.GetFloat(musicVolume, out float musicValue))
        {
            musicSlider.value = Mathf.Pow(10f, musicValue / 20f);
        }

        if (sfxSlider != null && audioMixer.GetFloat(sfxVolume, out float sfxValue))
        {
            sfxSlider.value = Mathf.Pow(10f, sfxValue / 20f);
        }
    }
}
