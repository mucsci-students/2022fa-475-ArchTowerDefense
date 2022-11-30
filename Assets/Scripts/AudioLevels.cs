using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLevels : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer soundMixer;

    public void SetMusicLevel (float sliderVal)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(sliderVal) * 20);
    }

    public void SetSoundLevel (float sliderVal)
    {
        soundMixer.SetFloat("SoundVol", Mathf.Log10(sliderVal) * 20);
    }
}
