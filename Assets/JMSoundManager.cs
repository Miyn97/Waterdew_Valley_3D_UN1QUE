using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class JMSoundManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterAudioSlider;
    public Slider sfxAudioSlider;
    public Slider bgmAudioSlider;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Start()
    {
        masterAudioSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxAudioSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmAudioSlider.onValueChanged.AddListener(SetBGMVolume);

        masterAudioSlider.value = masterVolume;
        sfxAudioSlider.value = sfxVolume;
        bgmAudioSlider.value = bgmVolume;

        ApplyVolume();
    }

    public void ApplyVolume()
    {
        masterMixer.SetFloat("Master", Mathf.Log10(masterVolume <= 0.0001f ? 0.0001f : masterVolume) * 20);

        masterMixer.SetFloat("BGM", Mathf.Log10(bgmVolume <= 0.0001f ? 0.0001f : bgmVolume) * 20);
        masterMixer.SetFloat("SFX", Mathf.Log10(sfxVolume <= 0.0001f ? 0.0001f : sfxVolume) * 20);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        ApplyVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        ApplyVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        ApplyVolume();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
