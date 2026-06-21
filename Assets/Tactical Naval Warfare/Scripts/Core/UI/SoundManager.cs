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

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource extrasAudioSource;

    [SerializeField] private string extraVolume = "ExtraVolume";
    [SerializeField] private string musicVolume = "MusicVolume";
    [SerializeField] private string sfxVolume = "SFXVolume";

    [Header("UI Slider")]
    [SerializeField] private Slider extrasSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [FoldoutGroup("Music Tracks", expanded: false)]
    [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

    [FoldoutGroup("SFX Clips", expanded: false)]
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();

    [FoldoutGroup("Extras", expanded: false)]
    [SerializeField] private List<AudioClip> extraClips = new List<AudioClip>();

    public void PlayMusicTrack(int index)
    {
        if (musicAudioSource == null) return;

        if (index >= 0 && index < musicTracks.Count && musicTracks[index] != null)
        {
            if (musicAudioSource.clip == musicTracks[index] && musicAudioSource.isPlaying)
            {
                return;
            }

            musicAudioSource.clip = musicTracks[index];
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }

    public void PlaySFX(int index)
    {
        if (sfxAudioSource == null) return;

        if (index >= 0 && index < sfxClips.Count && sfxClips[index] != null)
        {
            sfxAudioSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void PlayExtra(int index)
    {
        if (extrasAudioSource == null) return;

        if (index >= 0 && index < extraClips.Count && extraClips[index] != null)
        {
            extrasAudioSource.PlayOneShot(extraClips[index]);
        }
    }

    void Start()
    {
        if(extrasSlider != null)
        {
            extrasSlider.onValueChanged.AddListener(SetExtraVolume);
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

        if (extrasSlider != null && audioMixer.GetFloat(extraVolume, out float extrasValue))
        {
            extrasSlider.value = Mathf.Pow(10f, extrasValue / 20f);
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
