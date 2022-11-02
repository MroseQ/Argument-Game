using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColoringText : MonoBehaviour
{
    public string text;
    public string[] convertedText;

    void LateUpdate()
    {
        StartCoroutine(EndFrame());
    }
    public IEnumerator EndFrame() // Making the rich text, and coloring the part separated by separators {;,*}.
    {
        text = GetComponent<TextMeshPro>().text;
        convertedText = text.Split(';');
        if(convertedText.Length == 1)
        {
            convertedText = text.Split('*');
        }
        text = convertedText[0];
        if (convertedText.Length > 1)
        {
            convertedText[1] = "<color=#ebd914>" + convertedText[1] + "</color>";
            text += convertedText[1];

            // Instanting the hitboxes.
            if (name == "LeftText" && !GameObject.Find("LeftHitbox(Clone)")) { Instantiate(Resources.Load("PreFabs/LeftHitbox")); }
            if (name == "MidText" && !GameObject.Find("MidHitbox(Clone)")) { Instantiate(Resources.Load("PreFabs/MidHitbox")); }
            if (name == "RightText" && !GameObject.Find("RightHitbox(Clone)")) { Instantiate(Resources.Load("PreFabs/RightHitbox")); }
        }
        if (convertedText.Length > 2) 
        {
            text += convertedText[2];
        }
        GetComponent<TextMeshPro>().text = text;
        yield return new WaitForEndOfFrame();
    }
}

