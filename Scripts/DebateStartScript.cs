using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DebateStartScript : MonoBehaviour
{
    private ChromaticAberration chromaticAberration;
    private ColorGrading colorGrading;
    [SerializeField] private PostProcessVolume postProcessVolume;
    public bool ended;
    void Start()
    {
        chromaticAberration = postProcessVolume.profile.GetSetting<ChromaticAberration>();
        colorGrading = postProcessVolume.profile.GetSetting<ColorGrading>();
    }

    void Update()
    {
        if (chromaticAberration.intensity.value > 0f)
        {
            chromaticAberration.intensity.value -= 1f * Time.deltaTime;
        }
        else
        {
            chromaticAberration.intensity.value = 0f;
        }
        if (colorGrading.temperature.value > 0f)
        {
            colorGrading.temperature.value -= 50f * Time.deltaTime;
        }
        else
        {
            colorGrading.temperature.value = 0f;
        }
        if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && !ended)
        {
            GameObject.Find("RightText").GetComponent<TextDisplayEffect>().isPlaying = false;
        }
    }
}
