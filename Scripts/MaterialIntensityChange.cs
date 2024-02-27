using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialIntensityChange : MonoBehaviour
{
    [SerializeField] private Material m;
    [SerializeField] private bool isLowering;
    private float intensity;
    [SerializeField] private float speed;
    void Start()
    {
        isLowering = false;
        m.SetColor("_EmissionColor", new(0.1764f, 0.1568f, 0));
    }

    void Update()
    {
        Color c = m.GetColor("_EmissionColor");
        if (c.g <= 0.078f)
        {
            isLowering = false;
        }else if (c.g >= 0.3f)
        {
            isLowering=true;
        }
        if (isLowering)
        {
           m.SetColor("_EmissionColor", new(c.r - Time.deltaTime * speed, c.g - Time.deltaTime * speed, c.b));
        }
        else
        {
            m.SetColor("_EmissionColor", new(c.r + Time.deltaTime * speed, c.g + Time.deltaTime * speed, c.b));
        }
        m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
    }
}
