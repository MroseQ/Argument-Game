using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float baseValue;
    [SerializeField] private StoryProgress SP;

    private void Start()
    {
        if (gameObject.name == "MusicSlider")
        {
            slider.value = SP.musicPrevValue;
        }
        else
        {
            slider.value = SP.characterPrevValue;
            SetVolume(slider);
        }
    }

    public void SetAudio()
    {
        SetVolume(slider, gameObject.name);
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
        SetVolume(slider, gameObject.name);
    }
    private void SetVolume(Slider slider, string type = "CharacterSlider")
    {
        AudioSource source;
        if (type == "MusicSlider")
        {
            source = Camera.main.GetComponents<AudioSource>()[0];
            source.volume = slider.value * baseValue;
            SP.musicPrevValue = slider.value;
        }
        else
        {
            source = Camera.main.GetComponents<AudioSource>()[1];
            source.volume = slider.value * 0.404f;
            SP.characterPrevValue = slider.value;
        }
        
    }
}
