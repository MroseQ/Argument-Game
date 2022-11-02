using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DebateStartScript : MonoBehaviour
{
    ChromaticAberration chromaticAberration;
    ColorGrading colorGrading;
    public PostProcessVolume postProcessVolume;
    public bool ended, swiftStart;
    public float time;
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
        if(colorGrading.temperature.value == 0f)
        {
            var color = GetComponent<TextMeshPro>().color;
            var fadeAmount = color.a - (Time.deltaTime *1f);
            if(fadeAmount < 0.02f)
            {
                fadeAmount = 0f;
            }
            GetComponent<TextMeshPro>().color = new Color(color.r, color.g, color.b, fadeAmount);
        }
        if (GetComponent<TextMeshPro>().color.a == 0f && !ended) //Starts the text and sprites loading.
        {
            ended = true;
            GameObject.Find("RightText").GetComponent<TextDisplayEffect>().isPlaying = false;
        }

        // Moves the "Debate Start" text.
        transform.Translate(new Vector3(time * 1.5f, 0));
        transform.Rotate(new Vector3(0, 0, time/2));

        if (!ended && colorGrading.temperature.value == 0f)
        {
            time -= time * 1.5f * Time.deltaTime;
            if(!swiftStart) StartCoroutine(SwiftMovement(name));
        }
        if (ended)
        {
            time = 0;
        }
    }

    public IEnumerator SwiftMovement(string name) // Shifts the position of a text.
    {
        swiftStart = true;
        for (int i = 1; i < 182; i++)
        {
            if (name == "Debate") transform.Translate(new Vector3(time * 20f / i, 0));
            else if (name == "Start") transform.Translate(new Vector3(-(time * 45f / i), 0));
            yield return new WaitForFixedUpdate();
        }
    }
}
