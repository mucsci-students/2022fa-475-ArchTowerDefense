using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioLevels : MonoBehaviour
{
    public AudioMixer musicMixer;
    public Slider musicSlider;
    public AudioMixer soundMixer;
    public Slider soundSlider;

    void Start ()
    {
        musicMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("musicVol"));
        musicSlider.value = PlayerPrefs.GetFloat("musicSliderVal");
        soundMixer.SetFloat("SoundVol", PlayerPrefs.GetFloat("soundVol"));
        soundSlider.value = PlayerPrefs.GetFloat("soundSliderVal");
    }

    void Update()
    {
        // float val;
        // bool works = musicMixer.GetFloat("MusicVol", out val);

        // if (works)
        // {
        //     print("MUSIC VOL: " + val);
        //     print("MUSIC SHOULD BE: " + PlayerPrefs.GetFloat("musicVol"));
        // }
        // else
        //     print("NO VOL CONTROL!");
    }

    public void SetMusicLevel (float sliderVal)
    {
        float form = Mathf.Log10(sliderVal) * 20;
        musicMixer.SetFloat("MusicVol", form);
        PlayerPrefs.SetFloat("musicVol", form);
        PlayerPrefs.SetFloat("musicSliderVal", sliderVal);
    }

    public void SetSoundLevel (float sliderVal)
    {
        float form = Mathf.Log10(sliderVal) * 20;
        soundMixer.SetFloat("SoundVol", form);
        PlayerPrefs.SetFloat("soundVol", form);
        PlayerPrefs.SetFloat("soundSliderVal", sliderVal);
    }
}
